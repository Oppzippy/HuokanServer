using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace HuokanServer.DataAccess.Repository.UserDiscordTokenRepository
{
	public class UserDiscordTokenRepository : DbRepositoryBase, IUserDiscordTokenRepository
	{
		public UserDiscordTokenRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

		public async Task<BackedUserDiscordToken> GetDiscordToken(Guid userId)
		{
			using IDbConnection dbConnection = GetDbConnection();
			try
			{
				return await dbConnection.QueryFirstAsync<BackedUserDiscordToken>(@"
					SELECT
						user_account.external_id,
						user_discord_token.token,
						user_discord_token.refresh_token,
						user_discord_token.created_at,
						user_discord_token.expires_at,
						user_discord_token.created_at
					FROM
						user_account
					INNER JOIN user_discord_token ON
						user_discord_token.user_id = user_account.id
					WHERE
						user_account.external_id = @UserId",
					new
					{
						UserId = userId,
					}
				);
			}
			catch (InvalidOperationException ex)
			{
				throw new ItemNotFoundException("The specified user or token does not exist.", ex);
			}
		}

		public async Task SetDiscordToken(Guid userId, UserDiscordToken token)
		{
			using IDbConnection dbConnection = GetDbConnection();
			try
			{
				await dbConnection.ExecuteAsync(@"
					INSERT INTO user_discord_token (
						user_id,
						token,
						refresh_token,
						expires_at,
						created_at
					) VALUES (
						(SELECT id FROM user_account WHERE external_id = @UserId),
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
						CreatedAt = DateTimeOffset.UtcNow,
					}
				);
			}
			catch (NpgsqlException ex)
			{
				if (ex.SqlState == PostgresErrorCodes.NotNullViolation)
				{
					throw new ItemNotFoundException("The specified user does not exist.", ex);
				}
				throw;
			}
		}
	}
}
