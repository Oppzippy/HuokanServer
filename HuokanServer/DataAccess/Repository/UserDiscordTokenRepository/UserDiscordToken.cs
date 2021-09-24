using System;

namespace HuokanServer.DataAccess.Repository.UserDiscordTokenRepository
{
	public record UserDiscordToken
	{
		public string Token { get; init; }
		public string RefreshToken { get; init; }
		public DateTime ExpiresAt { get; init; }
	}
}
