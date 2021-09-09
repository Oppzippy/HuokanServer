using System;

namespace HuokanServer.Models.Repository.UserRepository
{
	public record BackedUser : User
	{
		public Guid Id { get; init; }
		public DateTime CreatedAt { get; init; }
	}
}
