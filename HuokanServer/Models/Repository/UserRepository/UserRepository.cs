using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.UserRepository
{
	public class UserRepository : DbRepositoryBase, IUserRepository
	{
		public UserRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedUser> GetUser(Guid id)
		{
			return await dbConnection.QueryFirstAsync<BackedUser>(@"
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
		}

		public async Task<BackedUser> FindOrCreateUser(User user)
		{
			// TODO do this in a transaction
			BackedUser existingUser = await FindUser(user);
			if (existingUser != null)
			{
				return existingUser;
			}
			return await CreateUser(user);
		}

		public async Task<BackedUser> FindUser(User user)
		{
			return await dbConnection.QueryFirstAsync<BackedUser>(@"
				SELECT
					external_id AS id,
					discord_user_id,
					created_at
				FROM
					user_account
				WHERE
					discord_user_id = @DiscordUserId",
				user
			);
		}

		public async Task<List<BackedUser>> FindUsersInOrganization(Guid organizationId)
		{
			return (await dbConnection.QueryAsync<BackedUser>(@"
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
			)).AsList();
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
			return await dbConnection.QueryFirstAsync<BackedUser>(@"
				INSERT INTO
					user_account (discord_user_id, created_at)
				VALUES
					(@DiscordUserId, @CreatedAt)
				RETURNING
					external_id AS id, discord_user_id, created_at",
				new
				{
					DiscordUserId = user.DiscordUserId,
					CreatedAt = DateTime.UtcNow,
				}
			);
		}

		public async Task SetDiscordOrganizations(Guid userId, List<ulong> guildIds)
		{
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
					GuildIds = guildIds,
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
					GuildIds = guildIds,
				},
				transaction
			);
			transaction.Commit();
		}
	}
}
