using System;

namespace HuokanServer.Models.Repository.ApiKeyRepository
{
	public record BackedApiKey : ApiKey
	{
		public Guid Id { get; init; }
		public string HashedKey { get; init; }
	}
}
