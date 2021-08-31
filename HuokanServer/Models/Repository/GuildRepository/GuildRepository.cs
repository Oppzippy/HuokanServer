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

		public async Task<List<BackedGuild>> FindGuilds(Guild guild)
		{
			var query = @"
				SELECT
					id, organization_id, 'name', realm, created_at
				FROM
					guild
				WHERE
					organization_id = @OrganizationId AND
					is_not_deleted = TRUE";
			if (guild.Name != null)
			{
				query += " AND 'name' = @Name";
			}
			if (guild.Realm != null)
			{
				query += " AND realm = @Realm";
			}

			return (await dbConnection.QueryAsync<BackedGuild>(query)).AsList();
		}

		public async Task<BackedGuild> FindGuild(Guild guild)
		{
			return await dbConnection.QueryFirstAsync<BackedGuild>(@"
				SELECT
					id, organization_id, 'name', realm, created_at
				FROM
					guild
				WHERE
					organization_id = @OrganizationId AND
					'name' = @Name AND
					realm = @Realm AND
					is_not_deleted = TRUE",
				guild
			);
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
					(@OrganizationId, @Name, @Realm, @CreatedAt)
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
					organization_id = @OrganizationId,
					'name' = @Name,
					realm = @Realm,
				WHERE
					id = @Id AND
					organization_id = @OrganizationId
				RETURNING id, organization_id, 'name', realm, created_at",
				guild
			);
		}

		public async Task DeleteGuild(BackedGuild guild)
		{
			await dbConnection.ExecuteAsync(@"
				UPDATE
					guild
				SET
					deleted_at = @DeletedAt
				WHERE
					id = @Id AND
					organization_id = @OrganizationId",
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
