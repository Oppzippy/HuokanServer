using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.GuildRepository
{
	public class GuildRepository : DbRepositoryBase
	{
		public GuildRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedGuild> GetGuild(Guid organizationId, Guid guildId)
		{
			return await dbConnection.QueryFirstAsync<BackedGuild>(@"
				SELECT
					guild.external_id AS id,
					organization.external_id AS organization_id,
					guild.'name',
					guild.realm,
					guild.created_at
				FROM
					guild
				INNER JOIN organization ON
					guild.organization_id = organization.id
				WHERE
					organization.external_id = @OrganizationId AND
					guild.external_id = @GuildId
					guild.is_not_deleted = TRUE",
				new
				{
					OrganizationId = organizationId,
					GuildId = guildId,
				}
			);
		}

		public async Task<List<BackedGuild>> FindGuilds(Guild guild)
		{
			var query = @"
				SELECT
					guild.external_id AS id,
					organization.external_id AS organization_id,
					guild.'name',
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
				query += " AND guild.'name' = @Name";
			}
			if (guild.Realm != null)
			{
				query += " AND guild.realm = @Realm";
			}

			return (await dbConnection.QueryAsync<BackedGuild>(query, guild)).AsList();
		}

		public async Task<BackedGuild> CreateGuild(Guild guild)
		{
			return await dbConnection.QueryFirstAsync<BackedGuild>(@"
				INSERT INTO
					guild (
						organization_id,
						'name',
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
					id, organization_id, 'name', realm, created_at",
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
					guild.'name' = @Name,
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
					guild.'name',
					guild.realm,
					guild.created_at",
				guild
			);
		}

		public async Task DeleteGuild(BackedGuild guild)
		{
			await dbConnection.ExecuteAsync(@"
				UPDATE
					guild
				SET
					guild.deleted_at = @DeletedAt
				FROM
					organization
				WHERE
					guild.organization_id = organization.id AND
					guild.external_id = @Id AND
					organization.external_id = @OrganizationId",
				new
				{
					Id = guild.Id,
					// Ensure guilds transferred to another org can't be deleted
					OrganizationId = guild.OrganizationId,
					DeletedAt = DateTime.UtcNow,
				}
			);
		}
	}
}
