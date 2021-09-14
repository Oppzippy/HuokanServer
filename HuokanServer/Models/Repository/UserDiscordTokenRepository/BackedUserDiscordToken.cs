using System;

namespace HuokanServer.Models.Repository.UserDiscordTokenRepository
{
	public record BackedUserDiscordToken : UserDiscordToken
	{
		public DateTime CreatedAt { get; init; }
	}
}
