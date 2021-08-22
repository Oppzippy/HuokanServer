using System;

namespace HuokanServer.Models.Repository.ApiKeyRepository
{
	public record ApiKey
	{
		public int UserId { get; init; }
		public DateTime CreatedAt { get; init; }
		public DateTime ExpiresAt { get; init; }
	}
}
