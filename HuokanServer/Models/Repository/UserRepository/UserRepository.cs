using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.UserRepository
{
	public class UserRepository : DbRepositoryBase, IUserRepository
	{
		public UserRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedUser> GetUser(Guid id)
		{
			DbBackedUser dbBackedUser = await dbConnection.QueryFirstAsync<DbBackedUser>(@"
				SELECT
					external_id AS id,
					discord_user_id,
					created_at
				FROM
					user_account
				WHERE
					external_id = @Id",
				new
				{
					Id = id,
				}
			);
			return dbBackedUser.ToBackedUser();
		}

		public async Task<BackedUser> FindOrCreateUser(User user)
		{
			// TODO do this in a transaction
			try
			{
				return await FindUser(user);
			}
			catch (NotFoundException)
			{
				return await CreateUser(user);
			}
		}

		public async Task<BackedUser> FindUser(User user)
		{
			try
			{
				DbBackedUser dbBackedUser = await dbConnection.QueryFirstAsync<DbBackedUser>(@"
				SELECT
					external_id AS id,
					discord_user_id,
					created_at
				FROM
					user_account
				WHERE
					discord_user_id = @DiscordUserId",
					new
					{
						DiscordUserId = Convert.ToDecimal(user.DiscordUserId),
					}
				);
				return dbBackedUser.ToBackedUser();
			}
			catch (InvalidOperationException ex)
			{
				throw new NotFoundException("The user could not be found.", ex);
			}
		}

		public async Task<List<BackedUser>> FindUsersInOrganization(Guid organizationId)
		{
			IEnumerable<DbBackedUser> dbBackedUsers = await dbConnection.QueryAsync<DbBackedUser>(@"
				SELECT
					user_account.external_id AS id,
					user_account.discord_user_id,
					user_account.created_at
				FROM
					user_account
				INNER JOIN organization_user_membership AS membership ON
					membership.user_id = user_account.id
				INNER JOIN organization ON
					organization.id = membership.organization_id
				WHERE
					organization.external_id = @OrganizationId",
				new
				{
					OrganizationId = organizationId,
				}
			);

			return dbBackedUsers.Select(dbBackedUser => dbBackedUser.ToBackedUser()).AsList();
		}

		public async Task<bool> IsUserInOrganization(Guid userId, Guid organizationId)
		{
			dynamic result = await dbConnection.QueryFirstAsync(@"
				SELECT
					COUNT(*) AS 'count'
				FROM
					user_account
				INNER JOIN organization_membership AS membership ON
					membership.user_id = user_account.id
				INNER JOIN organization ON
					organization.id = membership.organization_id
				WHERE
					user_account.external_id = @UserId AND
					organization.external_id = @OrganizationId",
				new
				{
					UserId = userId,
					OrganizationId = organizationId,
				}
			);
			return result.count > 0;
		}

		public async Task<BackedUser> CreateUser(User user)
		{
			DbBackedUser dbBackedUser = await dbConnection.QueryFirstAsync<DbBackedUser>(@"
				INSERT INTO
					user_account (discord_user_id, created_at)
				VALUES
					(@DiscordUserId, @CreatedAt)
				RETURNING
					external_id AS id, discord_user_id, created_at",
				new
				{
					DiscordUserId = Convert.ToDecimal(user.DiscordUserId),
					CreatedAt = DateTime.UtcNow,
				}
			);
			return dbBackedUser.ToBackedUser();
		}

		public async Task SetDiscordOrganizations(Guid userId, List<ulong> guildIds)
		{
			var decimalGuildIds = guildIds.Select(Convert.ToDecimal).ToList();

			using IDbTransaction transaction = dbConnection.BeginTransaction();
			await dbConnection.ExecuteAsync(@"
				DELETE FROM
					organization_user_membership AS membership
				USING
					organization, user_account
				WHERE
					membership.organization_id = organization.id AND
					membership.user_id = user_account.id AND
					user_account.external_id = @UserId AND
					organization.discord_guild_id != ANY(@GuildIds::NUMERIC ARRAY) AND
					organization.discord_guild_id IS NOT NULL",
				new
				{
					UserId = userId,
					GuildIds = decimalGuildIds,
				}
			);
			await dbConnection.ExecuteAsync(@"
				INSERT INTO organization_user_membership (
					organization_id,
					user_id
				) VALUES (
					SELECT
						organization.id,
						(SELECT id FROM user_account WHERE external_id = @UserId)
					FROM
						organization
					WHERE
						discord_guild_id = ANY(@GuildIds::NUMERIC ARRAY)
				)
				ON CONFLICT (organization_id, user_id) DO NOTHING",
				new
				{
					UserId = userId,
					GuildIds = decimalGuildIds,
				},
				transaction
			);
			transaction.Commit();
		}

		private record DbBackedUser
		{
			public Guid Id { get; init; }
			public decimal DiscordUserId { get; init; }
			public DateTime CreatedAt { get; init; }

			public BackedUser ToBackedUser()
			{
				return new BackedUser()
				{
					Id = Id,
					DiscordUserId = Convert.ToUInt64(DiscordUserId),
					CreatedAt = CreatedAt,
				};
			}
		}
	}
}
