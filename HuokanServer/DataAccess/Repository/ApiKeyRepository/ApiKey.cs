using System;

namespace HuokanServer.DataAccess.Repository.ApiKeyRepository;

public record ApiKey
{
	public Guid UserId { get; init; }
	public DateTimeOffset ExpiresAt { get; init; }
}
