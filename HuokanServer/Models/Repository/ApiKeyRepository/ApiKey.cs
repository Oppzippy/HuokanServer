using System;

namespace HuokanServer.Models.Repository.ApiKeyRepository
{
	public record ApiKey
	{
		public Guid UserId { get; init; }
		public DateTime CreatedAt { get; init; }
		public DateTime ExpiresAt { get; init; }
	}
}
