using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.GuildRepository
{
	public class GuildRepository : DbRepositoryBase, IGuildRepository
	{
		public GuildRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedGuild> GetGuild(Guid organizationId, Guid guildId)
		{
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
				throw new NotFoundException("The specified guild does not exist.", ex);
			}
		}

		public async Task<List<BackedGuild>> FindGuilds(Guid organizationId, GuildFilter guild)
		{
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
			return await dbConnection.QueryFirstAsync<BackedGuild>(@"
				INSERT INTO
					guild (
						organization_id,
						name,
						realm,
						created_at
					)
				VALUES
					(
						(SELECT id FROM organization WHERE external_id = @OrganizationId),
						@Name,
						@Realm,
						@CreatedAt
					)
				RETURNING
					external_id AS id, @OrganizationId AS organization_id, name, realm, created_at",
				new
				{
					OrganizationId = guild.OrganizationId,
					Name = guild.Name,
					Realm = guild.Realm,
					CreatedAt = DateTime.UtcNow,
				}
			);
		}

		public async Task<BackedGuild> UpdateGuild(BackedGuild guild)
		{
			return await dbConnection.QueryFirstAsync<BackedGuild>(@"
				UPDATE
					guild
				SET
					guild.organization_id = (SELECT id FROM organization WHERE external_id = @OrganizationId),
					guild.name = @Name,
					guild.realm = @Realm,
				FROM
					organization
				WHERE
					guild.organization_id = organization.id AND
					guild.external_id = @Id AND
					organization.external_id = @OrganizationId
				RETURNING
					guild.external_id AS id,
					organization.external_id AS organization_id,
					guild.name,
					guild.realm,
					guild.created_at",
				guild
			);
		}

		public async Task DeleteGuild(Guid organizationId, Guid guildId)
		{
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
					DeletedAt = DateTime.UtcNow,
				}
			);
			if (rowsAffected == 0)
			{
				throw new NotFoundException("The specified guild does not exist.");
			}
		}
	}
}
