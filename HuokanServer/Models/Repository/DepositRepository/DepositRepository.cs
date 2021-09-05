using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.DepositRepository
{
	public class DepositRepository : DbRepositoryBase
	{
		public DepositRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<List<BackedDeposit>> GetDeposits(Guid organizationId, Guid guildId)
		{
			var results = await dbConnection.QueryAsync<BackedDeposit>(@"
				WITH RECURSIVE node AS (
					(
						SELECT
							graph_node.id AS id,
							(SELECT COUNT(*) FROM deposit_node_endorsement AS dne WHERE dne.node_id = graph_node.id) AS endorsements
						FROM
							graph
						INNER JOIN guild ON
							guild.guild_bank_graph_id = graph.id
						INNER JOIN organization ON
							organization.id = guild.organization_id
						INNER JOIN graph_node ON
							graph_node.graph_id = graph.id
						LEFT OUTER JOIN graph_edge ON
							graph_edge.start_node_id = graph_node.id
						WHERE
							organization.external_id = @OrganizationId AND
							guild.external_id = @GuildId
						ORDER BY endorsements DESC
						LIMIT 1
					)
					UNION
					(
						SELECT 
							ge.end_node_id AS id,
							(SELECT COUNT(*) FROM deposit_node_endorsement AS dne WHERE dne.node_id = ge.end_node_id) AS endorsements
						FROM graph_node AS gn
						INNER JOIN graph_edge AS ge ON ge.start_node_id = gn.id
						INNER JOIN node AS d ON d.node_id = gn.id
						ORDER BY endorsements DESC
						LIMIT 1
					)
				) SELECT
					node.id,
					node.endorsements::INTEGER,
					deposit_node.character_name,
					deposit_node.deposit_in_copper
				FROM node INNER JOIN deposit_node ON deposit_node.node_id = node.id",
				new
				{
					OrganizationId = organizationId,
					GuildId = guildId,
				}
			);
			return results.AsList();
		}

		public async Task Import(Guid organizationId, Guid guildId, Guid userId, List<Deposit> deposits)
		{
			var sequences = await FindDepositSequence(organizationId, guildId, deposits);
			var maxLength = sequences.Max(sequence => sequence.Count);
			var matchedSequences = sequences.Where(sequence => sequence.Count == maxLength);

			foreach (var sequence in matchedSequences)
			{
				await Task.WhenAll(
					CreateEndorsements(userId, sequence),
					ImportSequence(organizationId, guildId, deposits, sequence)
				);
			}
		}

		private async Task<List<List<int>>> FindDepositSequence(Guid organizationId, Guid guildId, List<Deposit> deposits)
		{
			if (deposits.Count == 0)
			{
				return new List<List<int>>();
			}
			var depositIds = await FindDepositSequenceStart(organizationId, guildId, deposits[0]);
			var sequences = await FindDepositSequenceRest(organizationId, guildId, deposits, depositIds);
			return sequences;
		}

		private async Task<List<int>> FindDepositSequenceStart(Guid organizationId, Guid guildId, Deposit firstDeposit)
		{
			var rows = await dbConnection.QueryAsync<FindDepositSequenceStartResult>(@"
				SELECT
					graph_node.id
				FROM
					graph
				INNER JOIN graph_node ON
					graph_node.graph_id = graph.id
				INNER JOIN deposit_node ON
					deposit_node.node_id = graph_node.id
				INNER JOIN guild ON
					guild.guild_bank_graph_id = graph.id
				INNER JOIN organization ON
					organization.id = guild.organization_id
				WHERE
					organization.external_id = @OrganizationId AND
					guild.external_id = @GuildId AND
					deposit_node.character_name = @CharacterName AND
					deposit_node.deposit_in_copper = @DepositInCopper AND
					deposit_node.guild_bank_copper = @GuildBankCopper",
				new
				{
					OrganizationId = organizationId,
					GuildId = guildId,
					CharacterName = firstDeposit.CharacterName,
					DepositInCopper = firstDeposit.DepositInCopper,
					GuildBankCopper = firstDeposit.GuildBankCopper,
				}
			);
			return rows.Select(row => row.Id).AsList();
		}

		private class FindDepositSequenceStartResult
		{
			public int Id { get; set; }
		}

		private async Task<List<List<int>>> FindDepositSequenceRest(Guid organizationId, Guid guildId, List<Deposit> deposits, List<int> depositIds)
		{
			// TODO we probably don't need to verify organization id and guild id since this won't change from parent to child
			// and we're only looking at children of nodes from the correct organization/guild
			var sequencesByLatestId = new Dictionary<int, List<int>>();
			foreach (var depositId in depositIds)
			{
				sequencesByLatestId[depositId] = new List<int> { depositId };
			}
			for (int i = 1; i < deposits.Count; i++)
			{
				var deposit = deposits[i];
				var nextDeposits = (await dbConnection.QueryAsync(@"
					SELECT
						graph_node.id,
						graph_edge.start_node_id AS prev_id
					FROM
						graph
					INNER JOIN graph_node ON
						graph_node.graph_id = graph.id
					INNER JOIN graph_edge ON
						graph_edge.end_node_id = graph_node.id
					INNER JOIN deposit_node ON
						deposit_node.node_id = graph_node.id
					INNER JOIN guild ON
						guild.guild_bank_graph_id = graph.id
					INNER JOIN organization ON
						organization.id = guild.organization_id
					WHERE
						organization.external_id = @OrganizationId AND
						guild.external_id = @GuildId AND
						graph_edge.start_node_id = ANY(@PreviousNodeIds::INTEGER ARRAY) AND
						deposit_node.character_name = @CharacterName AND
						deposit_node.deposit_in_copper = @DepositInCopper AND
						deposit_node.guild_bank_copper = @GuildBankCopper",
					new
					{
						OrganizationId = organizationId,
						GuildId = guildId,
						PreviousNodeIds = depositIds,
						CharacterName = deposit.CharacterName,
						DepositInCopper = deposit.DepositInCopper,
						GuildBankCopper = deposit.GuildBankCopper,
					}
				)).AsList();
				if (nextDeposits.Count == 0)
				{
					break;
				}
				foreach (var nextDeposit in nextDeposits)
				{
					sequencesByLatestId[nextDeposit.Id] = sequencesByLatestId[nextDeposit.PrevId];
					sequencesByLatestId[nextDeposit.Id].Add(nextDeposit.Id);
					sequencesByLatestId.Remove(nextDeposit.PrevId);
				}
			}
			return sequencesByLatestId.Values.AsList();
		}

		private async Task CreateEndorsements(Guid userId, List<int> nodeIds)
		{
			using var transaction = dbConnection.BeginTransaction();
			foreach (var nodeId in nodeIds)
			{
				await dbConnection.ExecuteAsync(@"
					INSERT INTO deposit_node_endorsement
						(node_id, user_id, created_at)
					VALUES (
						@NodeId,
						(SELECT id FROM user WHERE external_id = @UserId),
						@CreatedAt
					)
					ON CONFLICT (node_id, user_id) DO NOTHING",
					new
					{
						NodeId = nodeId,
						UserId = userId,
						CreatedAt = DateTime.UtcNow,
					},
					transaction
				);
			}
			transaction.Commit();
		}

		private async Task ImportSequence(Guid organizationId, Guid guildId, List<Deposit> deposits, List<int> sequence)
		{
			using IDbTransaction transaction = dbConnection.BeginTransaction();
			int? prevNodeId = null;
			foreach (var deposit in deposits.GetRange(sequence.Count, sequence.Count - deposits.Count))
			{
				var result = await CreateDeposit(new CreateDepositsArgs
				{
					ParentNodeId = prevNodeId,
					GuildId = guildId,
					CharacterName = deposit.CharacterName,
					DepositInCopper = deposit.DepositInCopper,
					GuildBankCopper = deposit.GuildBankCopper,
					CreatedAt = DateTime.UtcNow,
				});
				prevNodeId = result.Id;
			}
			transaction.Commit();
		}

		private async Task<bool> DoesOrganizationContainGuild(Guid organizationId, Guid guildId, IDbTransaction transaction)
		{
			dynamic result = await dbConnection.QueryFirstAsync(@"
				SELECT
					COUNT(*) AS 'count'
				FROM
					organization
				INNER JOIN guild ON
					guild.organization_id = organization.id
				WHERE
					organization.external_id = @OrganizationId AND
					guild.external_id = @GuildId",
				new
				{
					OrganizationId = organizationId,
					GuildId = guildId,
				},
				transaction
			);
			return result.count > 0;
		}

		private async Task<CreateDepositResult> CreateDeposit(CreateDepositsArgs args)
		{
			return await dbConnection.QueryFirstAsync<CreateDepositResult>(@"
				WITH gn AS (
					INSERT INTO graph_node (graph_id, created_at)
					VALUES (
						(SELECT guild_bank_graph_id FROM guild WHERE external_id = @GuildId),
						@CreatedAt
					)
					RETURNING id, created_at
				),
				dn AS (
					INSERT INTO deposit_node (node_id, character_name, deposit_in_copper, guild_bank_copper)
					VALUES (
						(SELECT id FROM gn),
						@CharacterName,
						@DepositInCopper,
						@GuildBankCopper
					)
				),
				ge AS (
					INSERT INTO graph_edge(start_node_id, end_node_id, created_at)
					(
						SELECT
							@ParentNodeId,
							(SELECT id FROM gn),
							(SELECT created_at FROM gn)
						WHERE
							@ParentNodeId IS NOT NULL
					)
				)
				SELECT id FROM gn",
				args
			);
		}
	}
}
