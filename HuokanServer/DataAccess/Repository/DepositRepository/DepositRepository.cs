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

	public async Task<List<BackedDeposit>> GetNewerDeposits(Guid organizationId, Guid guildId, Guid? relativeNodeId,
		int limit)
	{
		return await GetDeposits(organizationId, guildId, relativeNodeId, limit, Direction.NEWER);
	}

	public async Task<List<BackedDeposit>> GetOlderDeposits(Guid organizationId, Guid guildId, Guid? relativeNodeId,
		int limit)
	{
		return await GetDeposits(organizationId, guildId, relativeNodeId, limit, Direction.OLDER);
	}

	public async Task<Guid?> GetHead(Guid organizationId, Guid guildId)
	{
		// XXX Very inefficient implementation. Don't select unnecessary rows. Cache the result for a short period of time.
		List<BackedDeposit> deposits = await GetDeposits(organizationId, guildId, null, int.MaxValue, Direction.NEWER);
		return deposits.Last()?.Id;
	}

	private async Task<List<BackedDeposit>> GetDeposits(Guid organizationId, Guid guildId, Guid? relativeNodeId,
		int limit, Direction direction)
	{
		await ThrowIfGuildDoesntExist(organizationId, guildId);
		if (limit <= 0)
		{
			return new List<BackedDeposit>();
		}

		if (direction == Direction.OLDER && relativeNodeId == null)
		{
			// If we are looking for older deposits without specifying a relative node, we want deposits older than
			// the newest deposit.
			relativeNodeId = await GetHead(organizationId, guildId);
		}

		var endNodeId = "end_node_id";
		var startNodeId = "start_node_id";
		if (direction == Direction.OLDER)
		{
			(endNodeId, startNodeId) = (startNodeId, endNodeId);
		}


		var orderByDirection = direction == Direction.NEWER ? "ASC" : "DESC";

		using IDbConnection dbConnection = GetDbConnection();
		var results = await dbConnection.QueryAsync<BackedDeposit>($@"
				WITH RECURSIVE node AS (
					(
						SELECT
							graph_node.id AS id,
							(SELECT COUNT(*) FROM deposit_node_endorsement AS dne WHERE dne.node_id = graph_node.id) AS endorsements,
							(
								SELECT PERCENTILE_DISC(0.5) WITHIN GROUP(ORDER BY approximate_deposit_timestamp)
								FROM deposit_node_endorsement AS dne WHERE dne.node_id = graph_node.id
							) AS approximate_deposit_timestamp,
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
							graph_edge.{endNodeId} AS id,
							(SELECT COUNT(*) FROM deposit_node_endorsement AS dne WHERE dne.node_id = graph_edge.end_node_id) AS endorsements,
							(
								SELECT PERCENTILE_DISC(0.5) WITHIN GROUP(ORDER BY approximate_deposit_timestamp)
								FROM deposit_node_endorsement AS dne WHERE dne.node_id = graph_edge.end_node_id
							) AS approximate_deposit_timestamp,
							node.recursion_count + 1
						FROM graph_node
						INNER JOIN graph_edge ON
							graph_edge.{endNodeId} = graph_node.id
						INNER JOIN node ON
							node.id = graph_edge.{startNodeId}
						WHERE
							node.recursion_count < @Limit
						ORDER BY endorsements DESC
						LIMIT 1
					)
				) SELECT
					deposit_node.external_id AS id,
					node.endorsements::INTEGER,
					node.approximate_deposit_timestamp,
					deposit_node.character_name,
					deposit_node.character_realm,
					deposit_node.deposit_in_copper
				FROM
					node
				INNER JOIN deposit_node ON
					deposit_node.node_id = node.id
				ORDER BY
					node.recursion_count {orderByDirection}",
			new
			{
				OrganizationId = organizationId,
				GuildId = guildId,
				AfterNodeId = relativeNodeId,
				Limit = relativeNodeId == null ? limit : Math.Max(limit + 1, limit),
			}
		);
		return relativeNodeId == null ? results.AsList() : results.Skip(1).AsList();
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
