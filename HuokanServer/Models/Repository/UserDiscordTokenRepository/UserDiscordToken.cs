using System;

namespace HuokanServer.Models.Repository.UserDiscordTokenRepository
{
	public record UserDiscordToken
	{
		public string Token { get; init; }
		public string RefreshToken { get; init; }
		public DateTime ExpiresAt { get; init; }
	}
}
