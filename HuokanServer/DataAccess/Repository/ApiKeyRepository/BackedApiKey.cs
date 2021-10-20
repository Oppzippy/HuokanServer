using System;

namespace HuokanServer.DataAccess.Repository.ApiKeyRepository
{
	public record BackedApiKey : ApiKey
	{
		public Guid Id { get; init; }
		public string KeyHash { get; init; }
		public DateTimeOffset CreatedAt { get; init; }
	}
}
