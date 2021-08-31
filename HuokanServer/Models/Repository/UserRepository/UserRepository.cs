using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.UserRepository
{
	public class UserRepository : DbRepositoryBase
	{
		public UserRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedUser> GetUser(int id)
		{
			return await dbConnection.QueryFirstAsync<BackedUser>(@"
				SELECT
					id,
					organization_id,
					discord_user_id,
					discord_token,
					created_at
				FROM
					user
				WHERE
					id = @Id",
				new
				{
					@Id = id,
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
					id,
					organization_id,
					discord_user_id,
					discord_token,
					created_at
				FROM
					user
				WHERE
					organization_id = @OrganizationId AND
					discord_user_id = @DiscordUserId",
				user
			);
		}

		public async Task<BackedUser> CreateUser(User user)
		{
			return await dbConnection.QueryFirstAsync<BackedUser>(@"
				INSERT INTO
					user (organization_id, discord_user_id, created_at)
				VALUES
					(@OrganizationId, @DiscordUserId, @CreatedAt)
				RETURNING
					id, organization_id, discord_user_id, discord_token, created_at",
				new
				{
					OrganizationId = user.OrganizationId,
					DiscordUserId = user.DiscordUserId,
					CreatedAt = DateTime.UtcNow,
				}
			);
		}

		public async Task UpdateDiscordToken(BackedUser user)
		{
			await dbConnection.ExecuteAsync(@"
				UPDATE
					user
				SET
					discord_token = @DiscordToken
				WHERE
					id = @Id",
				user
			);
		}
	}
}
