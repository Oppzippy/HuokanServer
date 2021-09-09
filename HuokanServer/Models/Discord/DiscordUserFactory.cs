using System;
using System.Threading.Tasks;

namespace HuokanServer.Models.Discord
{
	public class DiscordUserFactory : IDiscordUserFactory
	{
		private readonly DiscordUserAuthenticationHandler _discordUserAuthenticationHandler;

		public DiscordUserFactory(DiscordUserAuthenticationHandler discordUserAuthenticationHandler)
		{
			_discordUserAuthenticationHandler = discordUserAuthenticationHandler;
		}

		public async Task<IDiscordUser> Create(Guid userId)
		{
			string token = await _discordUserAuthenticationHandler.GetToken(userId);
			var user = new DiscordUser();
			// TODO try to authenticate, catch exception and check if token was invalid
			// If it's invalid, force refresh the token. If the refresh token is invalid, delete it alltogether and log the user out.
			await user.Authenticate(token);
			return user;
		}

		public async Task<IDiscordUser> Create(string token)
		{
			var user = new DiscordUser();
			await user.Authenticate(token);
			return user;
		}
	}
}
