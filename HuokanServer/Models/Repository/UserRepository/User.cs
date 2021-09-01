using System;

namespace HuokanServer.Models.Repository.UserRepository
{
	public record User
	{
		public Guid OrganizationId { get; init; }
		public ulong DiscordUserId { get; init; }
	}
}
