using System;

namespace HuokanServer.DataAccess.Repository.UserRepository;

public record BackedUser : User
{
	public Guid Id { get; init; }
	public DateTimeOffset CreatedAt { get; init; }
}
