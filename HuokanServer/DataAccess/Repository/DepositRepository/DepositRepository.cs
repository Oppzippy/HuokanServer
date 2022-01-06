using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.DataAccess.Repository.DepositRepository
{
	public class DepositRepository : DbRepositoryBase, IDepositRepository
	{
		private readonly IDepositImportExecutorFactory _depositImportExecutorFactory;

		public DepositRepository(
			IDbConnectionFactory dbConnectionFactory,
			IDepositImportExecutorFactory depositImportExecutorFactory
		) : base(dbConnectionFactory)
		{
			_depositImportExecutorFactory = depositImportExecutorFactory;
		}

		public async Task<List<BackedDeposit>> GetDeposits(Guid organizationId, Guid guildId)
		{
			using IDbConnection dbConnection = GetDbConnection();
			// Throw exception if the guild does not exist
			try
			{
				await dbConnection.QueryFirstAsync(@"
				SELECT 1
				FROM
					organization
				INNER JOIN guild ON
					guild.organization_id = organization.id
				WHERE
					guild.external_id = @GuildId AND
					organization.external_id = @OrganizationId",
					new
					{
						OrganizationId = organizationId,
						GuildId = guildId,
					}
				);
			}
			catch (InvalidOperationException ex)
			{
				throw new ItemNotFoundException("The guild does not exist.", ex);
			}
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
							graph_edge.end_node_id AS id,
							(SELECT COUNT(*) FROM deposit_node_endorsement AS dne WHERE dne.node_id = graph_edge.end_node_id) AS endorsements
						FROM graph_node
						INNER JOIN graph_edge ON graph_edge.start_node_id = graph_node.id
						INNER JOIN node ON node.id = graph_node.id
						ORDER BY endorsements DESC
						LIMIT 1
					)
				) SELECT
					node.id,
					node.endorsements::INTEGER,
					deposit_node.character_name,
					deposit_node.character_realm,
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
			IDepositImportExecutor executor = _depositImportExecutorFactory.Create();
			await executor.Import(organizationId, guildId, userId, deposits);
		}
	}
}
