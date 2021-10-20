using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace HuokanServer.DataAccess.Repository.GuildRepository
{
	public class GuildRepository : DbRepositoryBase, IGuildRepository
	{
		public GuildRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

		public async Task<BackedGuild> GetGuild(Guid organizationId, Guid guildId)
		{
			using IDbConnection dbConnection = GetDbConnection();
			try
			{
				return await dbConnection.QueryFirstAsync<BackedGuild>(@"
					SELECT
						guild.external_id AS id,
						organization.external_id AS organization_id,
						guild.name,
						guild.realm,
						guild.created_at
					FROM
						guild
					INNER JOIN organization ON
						guild.organization_id = organization.id
					WHERE
						organization.external_id = @OrganizationId AND
						guild.external_id = @GuildId AND
						guild.is_not_deleted = TRUE",
					new
					{
						OrganizationId = organizationId,
						GuildId = guildId,
					}
				);
			}
			catch (InvalidOperationException ex)
			{
				throw new ItemNotFoundException("The specified guild does not exist.", ex);
			}
		}

		public async Task<List<BackedGuild>> FindGuilds(Guid organizationId, GuildFilter guild)
		{
			using IDbConnection dbConnection = GetDbConnection();
			var query = @"
				SELECT
					guild.external_id AS id,
					organization.external_id AS organization_id,
					guild.name,
					guild.realm,
					guild.created_at
				FROM
					guild
				INNER JOIN organization ON
					guild.organization_id = organization.id
				WHERE
					organization.external_id = @OrganizationId AND
					is_not_deleted = TRUE";
			if (guild.Name != null)
			{
				query += " AND guild.name = @GuildName";
			}
			if (guild.Realm != null)
			{
				query += " AND guild.realm = @GuildRealm";
			}

			IEnumerable<BackedGuild> guilds = await dbConnection.QueryAsync<BackedGuild>(query, new
			{
				OrganizationId = organizationId,
				GuildName = guild.Name,
				GuildRealm = guild.Realm,
			});
			return guilds.AsList();
		}

		public async Task<BackedGuild> CreateGuild(Guild guild)
		{
			using IDbConnection dbConnection = GetDbConnection();
			using IDbTransaction transaction = dbConnection.BeginTransaction();
			try
			{
				// TODO don't use dynamic
				dynamic guildBankGraph = await dbConnection.QueryFirstAsync(@"
					INSERT INTO graph (
						id
					) VALUES (
						default
					)
					RETURNING id",
					transaction: transaction
				);
				BackedGuild backedGuild = await dbConnection.QueryFirstAsync<BackedGuild>(@"
					INSERT INTO
						guild (
							organization_id,
							name,
							realm,
							guild_bank_graph_id,
							created_at
						)
					VALUES
						(
							(SELECT id FROM organization WHERE external_id = @OrganizationId),
							@Name,
							@Realm,
							@GuildBankGraphId,
							@CreatedAt
						)
					RETURNING
						external_id AS id, @OrganizationId AS organization_id, name, realm, created_at",
					new
					{
						OrganizationId = guild.OrganizationId,
						Name = guild.Name,
						Realm = guild.Realm,
						GuildBankGraphId = guildBankGraph.id,
						CreatedAt = DateTimeOffset.UtcNow,
					}
				);
				transaction.Commit();
				return backedGuild;
			}
			catch (NpgsqlException ex)
			{
				if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
				{
					throw new DuplicateItemException("A guild with the supplied name and realm already exists.", ex);
				}
				throw;
			}
		}

		public async Task<BackedGuild> UpdateGuild(BackedGuild guild)
		{
			using IDbConnection dbConnection = GetDbConnection();
			try
			{
				return await dbConnection.QueryFirstAsync<BackedGuild>(@"
					UPDATE
						guild
					SET
						name = @GuildName,
						realm = @GuildRealm
					FROM
						organization
					WHERE
						guild.organization_id = organization.id AND
						organization.external_id = @OrganizationId AND
						guild.external_id = @GuildId AND
						guild.deleted_at IS NULL
					RETURNING
						guild.external_id AS id,
						organization.external_id AS organization_id,
						guild.name,
						guild.realm,
						guild.created_at",
					new
					{
						GuildId = guild.Id,
						GuildName = guild.Name,
						GuildRealm = guild.Realm,
						OrganizationId = guild.OrganizationId,
					}
				);
			}
			catch (InvalidOperationException ex)
			{
				throw new ItemNotFoundException("The guild does not exist.", ex);
			}
		}

		public async Task DeleteGuild(Guid organizationId, Guid guildId)
		{
			using IDbConnection dbConnection = GetDbConnection();
			int rowsAffected = await dbConnection.ExecuteAsync(@"
				UPDATE
					guild
				SET
					deleted_at = @DeletedAt
				FROM
					organization
				WHERE
					guild.organization_id = organization.id AND
					guild.external_id = @GuildId AND
					organization.external_id = @OrganizationId AND
					guild.deleted_at IS NULL",
				new
				{
					OrganizationId = organizationId,
					GuildId = guildId,
					DeletedAt = DateTimeOffset.UtcNow,
				}
			);
			if (rowsAffected == 0)
			{
				throw new ItemNotFoundException("The specified guild does not exist.");
			}
		}
	}
}
