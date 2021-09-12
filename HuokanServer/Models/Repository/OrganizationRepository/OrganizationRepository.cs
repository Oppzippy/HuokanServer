using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace HuokanServer.Models.Repository.OrganizationRepository
{
	public class OrganizationRepository : DbRepositoryBase, IOrganizationRepository
	{
		public OrganizationRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

		public async Task<BackedOrganization> GetOrganization(Guid organizationId)
		{
			using IDbConnection dbConnection = GetDbConnection();
			DbBackedOrganization org = await dbConnection.QueryFirstAsync<DbBackedOrganization>(@"
				SELECT
					external_id AS id,
					name,
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
			return org.ToBackedOrganization();
		}

		public async Task<BackedOrganization> FindOrganization(string slug)
		{
			using IDbConnection dbConnection = GetDbConnection();
			try
			{

				DbBackedOrganization org = await dbConnection.QueryFirstAsync<DbBackedOrganization>(@"
				SELECT
					external_id AS id,
					name,
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
				return org.ToBackedOrganization();
			}
			catch (InvalidOperationException ex)
			{
				throw new ItemNotFoundException("An organization with the provided slug could not be found.", ex);
			}
		}

		public async Task<BackedOrganization> FindOrganization(ulong discordGuildId)
		{
			using IDbConnection dbConnection = GetDbConnection();
			try
			{
				DbBackedOrganization org = await dbConnection.QueryFirstAsync<DbBackedOrganization>(@"
				SELECT
					external_id AS id,
					name,
					slug,
					discord_guild_id,
					created_at
				FROM
					organization
				WHERE
					discord_guild_id = @DiscordGuildId",
					new
					{
						DiscordGuildId = Convert.ToDecimal(discordGuildId),
					}
				);
				return org.ToBackedOrganization();
			}
			catch (InvalidOperationException ex)
			{
				throw new ItemNotFoundException("An organization with the provided discord guild id could not be found.", ex);
			}
		}

		public async Task<List<BackedOrganization>> FindOrganizationsContainingUser(Guid userId)
		{
			using IDbConnection dbConnection = GetDbConnection();
			IEnumerable<DbBackedOrganization> organizations = await dbConnection.QueryAsync<DbBackedOrganization>(@"
				SELECT
					organization.external_id AS id,
					organization.name,
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
			);

			return organizations.Select(org => org.ToBackedOrganization()).AsList();
		}

		public async Task<BackedOrganization> CreateOrganization(Organization organization)
		{
			using IDbConnection dbConnection = GetDbConnection();
			try
			{
				DbBackedOrganization organizations = await dbConnection.QueryFirstAsync<DbBackedOrganization>(@"
					INSERT INTO organization (name, slug, discord_guild_id, created_at)
					VALUES (@Name, @Slug, @DiscordGuildId, @CreatedAt)
					RETURNING external_id AS id, name, slug, discord_guild_id, created_at",
					new
					{
						Name = organization.Name,
						Slug = organization.Slug,
						DiscordGuildId = Convert.ToDecimal(organization.DiscordGuildId),
						CreatedAt = DateTime.UtcNow,
					}
				);
				return organizations.ToBackedOrganization();
			}
			catch (PostgresException ex)
			{
				if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
				{
					throw new DuplicateItemException("An organization with the supplied slug or discord guild id already exists.", ex);
				}
				throw;
			}
		}

		private record DbBackedOrganization
		{
			public string Name { get; init; }
			public string Slug { get; init; }
			public decimal DiscordGuildId { get; init; }
			public Guid Id { get; init; }
			public DateTime CreatedAt { get; init; }

			public BackedOrganization ToBackedOrganization()
			{
				return new BackedOrganization()
				{
					Name = Name,
					Slug = Slug,
					DiscordGuildId = Convert.ToUInt64(DiscordGuildId),
					Id = Id,
					CreatedAt = CreatedAt,
				};
			}
		}
	}
}
