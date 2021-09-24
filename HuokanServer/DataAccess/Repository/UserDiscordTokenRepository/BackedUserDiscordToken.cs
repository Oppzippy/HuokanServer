using System;

namespace HuokanServer.DataAccess.Repository.UserDiscordTokenRepository
{
	public record BackedUserDiscordToken : UserDiscordToken
	{
		public DateTime CreatedAt { get; init; }
	}
}
