using System;

namespace HuokanServer.Models.Repository.UserRepository
{
	public record User
	{
		public ulong DiscordUserId { get; init; }
	}
}
