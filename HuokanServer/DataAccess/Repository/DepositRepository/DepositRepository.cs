using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.DataAccess.Repository.DepositRepository;

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

	public async Task<List<BackedDeposit>> GetDeposits(Guid organizationId, Guid guildId, int limit = int.MaxValue)
	{
		return await GetDepositsAfter(organizationId, guildId, null, limit);
	}

	public async Task<List<BackedDeposit>> GetDepositsAfter(Guid organizationId, Guid guildId, Guid? afterNodeId,
		int limit = int.MaxValue)
	{
		await ThrowIfGuildDoesntExist(organizationId, guildId);
		if (limit <= 0)
		{
			return new List<BackedDeposit>();
		}

		using IDbConnection dbConnection = GetDbConnection();
		var results = await dbConnection.QueryAsync<BackedDeposit>(@"
				WITH RECURSIVE node AS (
					(
						SELECT
							graph_node.id AS id,
							(SELECT COUNT(*) FROM deposit_node_endorsement AS dne WHERE dne.node_id = graph_node.id) AS endorsements,
							1 as recursion_count
						FROM
							graph
						INNER JOIN guild ON
							guild.guild_bank_graph_id = graph.id
						INNER JOIN organization ON
							organization.id = guild.organization_id
						INNER JOIN graph_node ON
							graph_node.graph_id = graph.id
						INNER JOIN deposit_node ON
							graph_node.id = deposit_node.node_id
						WHERE
							organization.external_id = @OrganizationId AND
							guild.external_id = @GuildId AND
							(
								(
									@AfterNodeId IS NULL AND
									NOT EXISTS(
										SELECT 1 FROM graph_edge
										WHERE
											graph_edge.end_node_id = graph_node.id
									)
								) OR deposit_node.external_id = @AfterNodeId
							)
						ORDER BY endorsements DESC
						LIMIT 1
					)
					UNION
					(
						SELECT 
							graph_edge.end_node_id AS id,
							(SELECT COUNT(*) FROM deposit_node_endorsement AS dne WHERE dne.node_id = graph_edge.end_node_id) AS endorsements,
							node.recursion_count + 1
						FROM graph_node
						INNER JOIN graph_edge ON
							graph_edge.start_node_id = graph_node.id
						INNER JOIN node ON
							node.id = graph_node.id
						WHERE
							node.recursion_count < @Limit
						ORDER BY endorsements DESC
						LIMIT 1
					)
				) SELECT
					deposit_node.external_id AS id,
					node.endorsements::INTEGER,
					deposit_node.character_name,
					deposit_node.character_realm,
					deposit_node.deposit_in_copper
				FROM
					node
				INNER JOIN deposit_node ON
					deposit_node.node_id = node.id
				ORDER BY
					node.recursion_count",
			new
			{
				OrganizationId = organizationId,
				GuildId = guildId,
				AfterNodeId = afterNodeId,
				Limit = afterNodeId == null ? limit : Math.Max(limit + 1, limit),
			}
		);
		return afterNodeId == null ? results.AsList() : results.Skip(1).AsList();
	}

	public async Task<BackedDeposit> GetDeposit(Guid organizationId, Guid guildId, Guid depositId, int offset = 0)
	{
		if (offset <= 0)
		{
			return await GetDepositBefore(organizationId, guildId, depositId, -offset);
		}

		throw new NotImplementedException();
	}

	private async Task<BackedDeposit> GetDepositBefore(Guid organizationId, Guid guildId, Guid referenceDepositId,
		int offset)
	{
		using IDbConnection dbConnection = GetDbConnection();
		try
		{
			return await dbConnection.QueryFirstAsync<BackedDeposit>(@"
				WITH RECURSIVE node AS (
					(
						SELECT
							graph_node.id,
							1 AS recursion_count
						FROM
							graph
						INNER JOIN guild ON
							guild.guild_bank_graph_id = graph.id
						INNER JOIN organization ON
							organization.id = guild.organization_id
						INNER JOIN graph_node ON
							graph_node.graph_id = graph.id
						INNER JOIN deposit_node ON
							graph_node.id = deposit_node.node_id
						WHERE
							organization.external_id = @OrganizationId AND
							guild.external_id = @GuildId AND
							deposit_node.external_id = @DepositId
					)
					UNION
					(
						SELECT
							graph_node.id,
							node.recursion_count + 1
						FROM
							graph_node
						INNER JOIN deposit_node ON
							graph_node.id = deposit_node.node_id
						INNER JOIN graph_edge ON 
							graph_edge.start_node_id = graph_node.id
						INNER JOIN node ON
							graph_edge.end_node_id = node.id
						WHERE
							node.recursion_count <= @Offset
					)
				)
				SELECT
					deposit_node.external_id AS id,
					(SELECT COUNT(*) FROM deposit_node_endorsement AS dne WHERE dne.node_id = node.id) AS endorsements,
					deposit_node.character_name,
					deposit_node.character_realm,
					deposit_node.deposit_in_copper
				FROM
					node
				INNER JOIN deposit_node ON
					node.id = deposit_node.node_id
				ORDER BY
					node.recursion_count DESC
				LIMIT 1",
				new
				{
					OrganizationId = organizationId,
					GuildId = guildId,
					DepositId = referenceDepositId,
					Offset = offset,
				});
		}
		catch (InvalidOperationException ex)
		{
			throw new ItemNotFoundException("The deposit does not exist.", ex);
		}
	}

	private async Task ThrowIfGuildDoesntExist(Guid organizationId, Guid guildId)
	{
		using IDbConnection dbConnection = GetDbConnection();
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
	}

	public async Task Import(Guid organizationId, Guid guildId, Guid userId, List<Deposit> deposits)
	{
		IDepositImportExecutor executor = _depositImportExecutorFactory.Create();
		await executor.Import(organizationId, guildId, userId, deposits);
	}
}
