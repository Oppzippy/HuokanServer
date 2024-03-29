using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.DataAccess.Repository.DepositRepository;

public class DepositImportExecutor : IDepositImportExecutor
{
	private readonly IDbConnectionFactory _connectionFactory;
	private IDbConnection _dbConnection;
	private IDbTransaction _transaction;

	public DepositImportExecutor(IDbConnectionFactory connectionFactory)
	{
		_connectionFactory = connectionFactory;
	}

	public async Task Import(Guid organizationId, Guid guildId, Guid userId, List<Deposit> deposits)
	{
		using IDbConnection dbConnection = _connectionFactory.Create();
		using IDbTransaction transaction = dbConnection.BeginTransaction();
		_dbConnection = dbConnection;
		_transaction = transaction;

		if (!await DoesOrganizationContainGuild(organizationId, guildId))
		{
			throw new ItemNotFoundException("Guild not found.");
		}

		List<List<int>> sequences = await FindDepositSequence(guildId, deposits);
		if (sequences.Any())
		{
			await ImportMergeIntoExistingSequences(guildId, userId, deposits, sequences);
		}
		else
		{
			await ImportAndEndorseSequence(guildId, userId, deposits, null);
		}

		_transaction.Commit();
	}


	private async Task<bool> DoesOrganizationContainGuild(Guid organizationId, Guid guildId)
	{
		dynamic result = await _dbConnection.QueryFirstAsync(@"
				SELECT
					COUNT(*) AS ""count""
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
			_transaction
		);
		return result.count > 0;
	}

	private async Task<List<List<int>>> FindDepositSequence(Guid guildId, List<Deposit> deposits)
	{
		if (deposits.Count == 0)
		{
			return new List<List<int>>();
		}
		List<int> depositIds = await FindDepositSequenceStart(guildId, deposits[0]);
		List<List<int>> sequences = await FindDepositSequenceRest(guildId, deposits, depositIds);
		return sequences;
	}

	private async Task<List<int>> FindDepositSequenceStart(Guid guildId, Deposit firstDeposit)
	{
		var rows = await _dbConnection.QueryAsync<FindDepositSequenceStartResult>(@"
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
				WHERE
					guild.external_id = @GuildId AND
					deposit_node.character_name = @CharacterName AND
					deposit_node.character_realm = @CharacterRealm AND
					deposit_node.deposit_in_copper = @DepositInCopper AND
					deposit_node.guild_bank_copper = @GuildBankCopper",
			new
			{
				GuildId = guildId,
				CharacterName = firstDeposit.CharacterName,
				CharacterRealm = firstDeposit.CharacterRealm,
				DepositInCopper = firstDeposit.DepositInCopper,
				GuildBankCopper = firstDeposit.GuildBankCopper,
			},
			_transaction
		);
		return rows.Select(row => row.Id).AsList();
	}

	private class FindDepositSequenceStartResult
	{
		public int Id { get; init; }
	}

	private async Task<List<List<int>>> FindDepositSequenceRest(Guid guildId, List<Deposit> deposits, List<int> depositIds)
	{
		// TODO we probably don't need to verify guild id in WHERE since this won't change from parent to child
		// and we're only looking at children of nodes from the correct guild
		var sequencesByLatestId = new Dictionary<int, List<int>>();
		foreach (int depositId in depositIds)
		{
			sequencesByLatestId[depositId] = new List<int> { depositId };
		}
		for (int i = 1; i < deposits.Count; i++)
		{
			Deposit deposit = deposits[i];
			// TODO don't use dynamic
			var nextDeposits = (await _dbConnection.QueryAsync(@"
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
					WHERE
						guild.external_id = @GuildId AND
						graph_edge.start_node_id = ANY(@PreviousNodeIds::INTEGER ARRAY) AND
						deposit_node.character_name = @CharacterName AND
						deposit_node.character_realm = @CharacterRealm AND
						deposit_node.deposit_in_copper = @DepositInCopper AND
						deposit_node.guild_bank_copper = @GuildBankCopper",
				new
				{
					GuildId = guildId,
					PreviousNodeIds = depositIds,
					CharacterName = deposit.CharacterName,
					CharacterRealm = deposit.CharacterRealm,
					DepositInCopper = deposit.DepositInCopper,
					GuildBankCopper = deposit.GuildBankCopper,
				},
				_transaction
			)).AsList();
			if (nextDeposits.Count == 0)
			{
				break;
			}
			foreach (var nextDeposit in nextDeposits)
			{
				sequencesByLatestId[nextDeposit.id] = sequencesByLatestId[nextDeposit.prev_id];
				sequencesByLatestId[nextDeposit.id].Add(nextDeposit.id);
				sequencesByLatestId.Remove(nextDeposit.prev_id);
			}
		}
		return sequencesByLatestId.Values.AsList();
	}

	private async Task ImportMergeIntoExistingSequences(Guid guildId, Guid userId, List<Deposit> deposits, List<List<int>> sequences)
	{
		int maxLength = sequences.Max(sequence => sequence.Count);
		IEnumerable<List<int>> matchedSequences = sequences.Where(sequence => sequence.Count == maxLength);

		foreach (List<int> sequence in matchedSequences)
		{
			List<Deposit> existingDeposits = deposits.GetRange(0, sequence.Count);
			List<Deposit> newDeposits = deposits.GetRange(sequence.Count, deposits.Count - sequence.Count);
			await CreateEndorsements(userId, sequence, existingDeposits);
			await ImportAndEndorseSequence(guildId, userId, newDeposits, sequence.Last());
		}
	}

	private async Task ImportAndEndorseSequence(Guid guildId, Guid userId, List<Deposit> deposits, int? parentNodeId)
	{
		List<int> nodeIds = await ImportSequence(guildId, deposits, parentNodeId);
		await CreateEndorsements(userId, nodeIds, deposits);
	}

	private async Task<List<int>> ImportSequence(Guid guildId, List<Deposit> deposits, int? parentNodeId)
	{
		var nodeIds = new List<int>();
		foreach (Deposit deposit in deposits)
		{
			CreateDepositResult result = await CreateDeposit(new CreateDepositsArgs
			{
				ParentNodeId = parentNodeId,
				GuildId = guildId,
				CharacterName = deposit.CharacterName,
				CharacterRealm = deposit.CharacterRealm,
				DepositInCopper = deposit.DepositInCopper,
				GuildBankCopper = deposit.GuildBankCopper,
				CreatedAt = DateTimeOffset.UtcNow,
			});
			parentNodeId = result.Id;
			nodeIds.Add(result.Id);
		}
		return nodeIds;
	}

	private async Task<CreateDepositResult> CreateDeposit(CreateDepositsArgs args)
	{
		return await _dbConnection.QueryFirstAsync<CreateDepositResult>(@"
				WITH gn AS (
					INSERT INTO graph_node (graph_id, created_at)
					VALUES (
						(SELECT guild_bank_graph_id FROM guild WHERE external_id = @GuildId),
						@CreatedAt
					)
					RETURNING id, created_at
				),
				dn AS (
					INSERT INTO deposit_node (node_id, character_name, character_realm, deposit_in_copper, guild_bank_copper)
					VALUES (
						(SELECT id FROM gn),
						@CharacterName,
						@CharacterRealm,
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
			args,
			_transaction
		);
	}
	
	private async Task CreateEndorsements(Guid userId, List<int> nodeIds, List<Deposit> deposits)
	{
		if (nodeIds.Count != deposits.Count)
		{
			throw new ArgumentException("nodeIds and deposits must have the same length");
		}
		for (int i = 0; i < nodeIds.Count; i++)
		{
			int nodeId = nodeIds[i];
			Deposit deposit = deposits[i];
			await _dbConnection.ExecuteAsync(@"
					INSERT INTO deposit_node_endorsement
						(node_id, user_id, created_at, approximate_deposit_timestamp)
					VALUES (
						@NodeId,
						(SELECT id FROM user_account WHERE external_id = @UserId),
						@CreatedAt,
						@ApproximateDepositTime
					)
					ON CONFLICT (node_id, user_id) DO NOTHING",
				new
				{
					NodeId = nodeId,
					UserId = userId,
					CreatedAt = DateTimeOffset.UtcNow,
					ApproximateDepositTime = deposit.ApproximateDepositTimestamp,
				},
				_transaction
			);
		}
	}
}
