using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.OrganizationRepository
{
	public class OrganizationRepository : DbRepositoryBase
	{
		public OrganizationRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedOrganization> FindOrganization(string slug)
		{
			return await dbConnection.QueryFirstAsync<BackedOrganization>(@"
				SELECT
					id,
					'name',
					slug,
					discord_guild_id,
					created_at
				FROM
					organization
				WHERE
					slug = @Slug",
				new
				{
					Slug = slug,
				}
			);
		}

		public async Task<BackedOrganization> FindOrganization(ulong discordGuildId)
		{
			return await dbConnection.QueryFirstAsync<BackedOrganization>(@"
				SELECT
					id,
					'name',
					slug,
					discord_guild_id,
					created_at
				FROM
					organization
				WHERE
					discord_guild_id = @DiscordGuildId",
				new
				{
					DiscordGuildId = discordGuildId,
				}
			);
		}

		public async Task CreateOrganization(Organization organization)
		{
			await dbConnection.ExecuteAsync(@"
				INSERT INTO organization ('name', slug, discord_guild_id, created_at)
				VALUES (@Name, @Slug, @DiscordGuildId, @CreatedAt)",
				new
				{
					Name = organization.Name,
					Slug = organization.Slug,
					DiscordGuildId = organization.DiscordGuildId,
					CreatedAt = DateTime.UtcNow,
				}
			);
		}
	}
}
