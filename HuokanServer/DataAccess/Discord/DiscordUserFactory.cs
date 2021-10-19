using System;
using System.Threading.Tasks;
using DSharpPlus.Exceptions;

namespace HuokanServer.DataAccess.Discord
{
	public class DiscordUserFactory : IDiscordUserFactory
	{
		private readonly IDiscordUserAuthenticationHandler _discordUserAuthenticationHandler;

		public DiscordUserFactory(IDiscordUserAuthenticationHandler discordUserAuthenticationHandler)
		{
			_discordUserAuthenticationHandler = discordUserAuthenticationHandler;
		}

		public async Task<IDiscordUser> Create(Guid userId)
		{
			string token = await _discordUserAuthenticationHandler.GetToken(userId);
			var user = new DiscordUser();
			try
			{
				await user.Authenticate(token);
			}
			catch (UnauthorizedException)
			{
				token = await _discordUserAuthenticationHandler.ForceRefreshToken(userId);
				await user.Authenticate(token);
			}
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
