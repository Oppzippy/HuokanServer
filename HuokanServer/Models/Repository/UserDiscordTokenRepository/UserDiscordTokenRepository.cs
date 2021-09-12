using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.UserDiscordTokenRepository
{
	public class UserDiscordTokenRepository : DbRepositoryBase, IUserDiscordTokenRepository
	{
		public UserDiscordTokenRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

		public async Task<UserDiscordToken> GetDiscordToken(Guid userId)
		{
			using IDbConnection dbConnection = GetDbConnection();
			return await dbConnection.QueryFirstAsync<UserDiscordToken>(@"
                SELECT
                    user.external_id,
                    user_discord_token.token,
                    user_discord_token.refresh_token,
                    user_discord_token.created_at,
                    user_discord_token.expires_at,
                    user_discord_token.created_at
                FROM
                    user
                INNER JOIN user_discord_token ON
                    user_discord_token.user_id = user.id
                WHERE
                    user.external_id = @UserId",
				new
				{
					UserId = userId,
				}
			);
		}

		public async Task SetDiscordToken(Guid userId, UserDiscordToken token)
		{
			using IDbConnection dbConnection = GetDbConnection();
			await dbConnection.QueryFirstAsync(@"
                INSERT INTO user_discord_token (
                    user_id,
                    token,
                    refresh_token,
                    expires_at,
                    created_at
                ) VALUES (
                    (SELECT id FROM user WHERE external_id = @UserId),
                    @Token,
                    @RefreshToken,
                    @ExpiresAt,
                    @CreatedAt
                )
                ON CONFLICT (user_id) DO UPDATE SET
                    token = excluded.token,
                    refresh_token = excluded.refresh_token,
                    expires_at = excluded.expires_at",
				new
				{
					UserId = userId,
					Token = token.Token,
					RefreshToken = token.RefreshToken,
					ExpiresAt = token.ExpiresAt,
					CreatedAt = DateTime.UtcNow,
				}
			);
		}
	}
}
