using System;

namespace HuokanServer.Models.Repository.UserRepository
{
	public record BackedUser : User
	{
		public int Id { get; init; }
		public string DiscordToken { get; init; }
		public DateTime CreatedAt { get; init; }
	}
}
