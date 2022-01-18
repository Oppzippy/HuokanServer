using System.Threading.Tasks;
using HuokanServer.DataAccess.Cache.DiscordUser;

namespace HuokanServer.DataAccess.Discord.User
{
	public class DiscordUserFactory : IUnknownDiscordUserFactory
	{
		private readonly IDiscordUserAuthenticationHandlerFactory _authenticationHandlerFactory;
		private readonly DiscordUserCache _cache;

		public DiscordUserFactory(IDiscordUserAuthenticationHandlerFactory discordUserAuthenticationHandler, DiscordUserCache cache)
		{
			_authenticationHandlerFactory = discordUserAuthenticationHandler;
			_cache = cache;
		}

		public Task<IDiscordUser> Create(string token)
		{
			IDiscordUserAuthenticationHandler authenticationHandler = _authenticationHandlerFactory.Create(token);
			return Task.FromResult<IDiscordUser>(new DiscordUser(authenticationHandler, token));
		}
	}
}
