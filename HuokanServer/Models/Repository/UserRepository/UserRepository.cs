using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.Models.Repository.UserRepository
{
	public class UserRepository : DbRepositoryBase
	{
		public UserRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedUser> GetUser(Guid id)
		{
			return await dbConnection.QueryFirstAsync<BackedUser>(@"
				SELECT
					external_id AS id,
					discord_user_id,
					discord_token,
					created_at
				FROM
					user
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
					discord_token,
					created_at
				FROM
					user
				WHERE
					discord_user_id = @DiscordUserId",
				user
			);
		}

		public async Task<List<BackedUser>> FindUsersInOrganization(Guid organizationId)
		{
			return (await dbConnection.QueryAsync<BackedUser>(@"
				SELECT
					user.external_id AS id,
					user.discord_user_id,
					user.discord_token,
					user.created_at
				FROM
					user
				INNER JOIN organization_user_membership AS membership
					ON membership.user_id = user.id
				INNER JOIN organization
					ON organization.id = membership.organization_id
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
					COUNT(*) AS count
				FROM
					user
				INNER JOIN organization_membership AS membership
					ON membership.user_id = user.id
				INNER JOIN organization
					ON organization.id = membership.organization_id
				WHERE
					user.external_id = @UserId AND
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
					user (discord_user_id, created_at)
				VALUES
					(@DiscordUserId, @CreatedAt)
				RETURNING
					external_id AS id, discord_user_id, discord_token, created_at",
				new
				{
					DiscordUserId = user.DiscordUserId,
					CreatedAt = DateTime.UtcNow,
				}
			);
		}

		public async Task<BackedUser> UpdateDiscordToken(Guid userId, string discordToken)
		{
			return await dbConnection.QueryFirstAsync<BackedUser>(@"
				UPDATE
					user
				SET
					discord_token = @DiscordToken
				WHERE
					external_id = @UserId
				RETURNING
					external_id AS id, discord_user_id, discord_token, created_at",
				new
				{
					UserId = userId,
					DiscordToken = discordToken,
				}
			);
		}
	}
}
