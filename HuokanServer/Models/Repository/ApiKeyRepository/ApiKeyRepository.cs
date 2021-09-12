using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.ApiKeyRepository
{
	public class ApiKeyRepository : DbRepositoryBase, IApiKeyRepository
	{
		private const int API_KEY_SIZE_IN_BYTES = 32;

		public ApiKeyRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

		public async Task<BackedApiKey> FindApiKey(string apiKey)
		{
			using IDbConnection dbConnection = GetDbConnection();
			var hash = HashApiKey(apiKey);
			return await dbConnection.QueryFirstAsync<BackedApiKey>(@"
                SELECT
                    user.external_id AS id
                FROM
                    unexpired_api_key
				INNER JOIN user ON
					unexpired_api_key.user_id = user.id
                WHERE
                    unexpired_api_key.hashed_key = @HashedKey",
				new
				{
					HashedKey = hash,
				}
			);
		}

		public async Task<string> CreateApiKey(ApiKey apiKey)
		{
			using IDbConnection dbConnection = GetDbConnection();
			var base64 = GenerateApiKey(API_KEY_SIZE_IN_BYTES);
			var hash = HashApiKey(base64);

			await dbConnection.ExecuteAsync(@"
                INSERT INTO api_key (
                    key_hash,
                    user_id,
                    created_at,
                    expires_at
                ) VALUES (
                    @KeyHash,
                    (SELECT id FROM user WHERE external_id = @UserId),
                    @CreatedAt
                    @ExpiresAt
                )",
				new
				{
					KeyHash = hash,
					UserId = apiKey.UserId,
					CreatedAt = apiKey.CreatedAt,
					ExpiresAt = apiKey.ExpiresAt,
				}
			);

			return base64;
		}

		private string GenerateApiKey(int sizeInBytes)
		{
			using var rng = new RNGCryptoServiceProvider();

			var apiKeyBytes = new byte[sizeInBytes];
			rng.GetBytes(apiKeyBytes);

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
