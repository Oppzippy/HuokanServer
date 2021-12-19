using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace HuokanServer.DataAccess.Repository.ApiKeyRepository
{
	public class ApiKeyRepository : DbRepositoryBase, IApiKeyRepository
	{
		private const int API_KEY_SIZE_IN_BYTES = 32;

		public ApiKeyRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

		public async Task<BackedApiKey> FindApiKey(string apiKey)
		{
			using IDbConnection dbConnection = GetDbConnection();
			var hash = HashApiKey(apiKey);
			try
			{
				// When the select user_account.external_id isn't in a subquery, AS id
				// causes it to be read as an INTEGER. I'm not sure why this is an issue here,
				// but the subquery properly makes it read as a uuid.
				return await dbConnection.QueryFirstAsync<BackedApiKey>(@"
					SELECT
						unexpired_api_key.external_id AS id,
						(SELECT user_account.external_id) AS user_id,
						unexpired_api_key.created_at,
						unexpired_api_key.expires_at
					FROM
						unexpired_api_key
					INNER JOIN user_account ON
						unexpired_api_key.user_id = user_account.id
					WHERE
						unexpired_api_key.key_hash = @KeyHash",
					new
					{
						KeyHash = hash,
					}
				);
			}
			catch (InvalidOperationException ex)
			{
				throw new ItemNotFoundException("The specified api key does not exist.", ex);
			}
		}

		public async Task<string> CreateApiKey(ApiKey apiKey)
		{
			using IDbConnection dbConnection = GetDbConnection();
			var base64 = GenerateApiKey(API_KEY_SIZE_IN_BYTES);
			var hash = HashApiKey(base64);

			try
			{
				await dbConnection.ExecuteAsync(@"
					INSERT INTO api_key (
						key_hash,
						user_id,
						created_at,
						expires_at
					) VALUES (
						@KeyHash,
						(SELECT id FROM user_account WHERE external_id = @UserId),
						@CreatedAt,
						@ExpiresAt
					)",
					new
					{
						KeyHash = hash,
						UserId = apiKey.UserId,
						CreatedAt = DateTimeOffset.UtcNow,
						ExpiresAt = apiKey.ExpiresAt.ToUniversalTime(),
					}
				);
			}
			catch (NpgsqlException ex)
			{
				if (ex.SqlState == PostgresErrorCodes.NotNullViolation)
				{
					throw new ItemNotFoundException("The specified users does not exist.", ex);
				}
				throw;
			}

			return base64;
		}

		private string GenerateApiKey(int sizeInBytes)
		{
			byte[] apiKeyBytes = RandomNumberGenerator.GetBytes(sizeInBytes);
			return Convert.ToBase64String(apiKeyBytes);
		}

		private string HashApiKey(string apiKey)
		{
			using var sha = SHA256.Create();
			var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(apiKey));
			return Convert.ToBase64String(hashBytes);
		}
	}

}
