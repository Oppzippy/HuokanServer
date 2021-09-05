using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HuokanServer.Models.Repository.UserRepository;

namespace HuokanServer.Models.Repository.OrganizationRepository
{
	public class OrganizationRepository : DbRepositoryBase
	{
		public OrganizationRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedOrganization> GetOrganization(Guid organizationId)
		{
			return await dbConnection.QueryFirstAsync<BackedOrganization>(@"
				SELECT
					external_id AS id,
					'name',
					slug,
					discord_guild_id,
					created_at
				FROM
					organization
				WHERE
					external_id = @Id",
				new
				{
					Id = organizationId,
				}
			);
		}

		public async Task<BackedOrganization> FindOrganization(string slug)
		{
			return await dbConnection.QueryFirstAsync<BackedOrganization>(@"
				SELECT
					external_id AS id,
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
					external_id AS id,
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

		public async Task<List<BackedOrganization>> FindOrganizationsContainingUser(Guid userId)
		{
			return (await dbConnection.QueryAsync<BackedOrganization>(@"
				SELECT
					organization.external_id AS id,
					organization.'name',
					organization.slug,
					organization.discord_guild_id,
					organization.created_at
				FROM
					organization
				INNER JOIN organization_user_membership AS membership ON
					membership.organization_id = organization.id
				INNER JOIN user ON
					user.id = membership.user_id
				WHERE
					user.external_id = @UserId",
				new
				{
					UserId = userId,
				}
			)).AsList();
		}

		public async Task<BackedOrganization> CreateOrganization(Organization organization)
		{
			return await dbConnection.QueryFirstAsync<BackedOrganization>(@"
				INSERT INTO organization ('name', slug, discord_guild_id, created_at)
				VALUES (@Name, @Slug, @DiscordGuildId, @CreatedAt)
				RETURNING external_id AS id, 'name', slug, discord_guild_id, created_at",
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
