using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord.User;

namespace HuokanServer.DataAccess.Cache.DiscordUser;

public class CachedDiscordUserFactory : IKnownDiscordUserFactory
{
	private readonly DiscordUserCache _discordUserCache;
	private readonly IUnknownDiscordUserFactory _discordUserFactory;
	private readonly IDiscordUserAuthenticationHandlerFactory _authenticationHandlerFactory;

	public CachedDiscordUserFactory(DiscordUserCache discordUserCache, IUnknownDiscordUserFactory discordUserFactory, IDiscordUserAuthenticationHandlerFactory authenticationHandlerFactory)
	{
		_discordUserCache = discordUserCache;
		_discordUserFactory = discordUserFactory;
		_authenticationHandlerFactory = authenticationHandlerFactory;
	}

	public Task<IDiscordUser> Create(Guid huokanUserId, ulong discordUserId)
	{
		return Task.FromResult<IDiscordUser>(new CachedDiscordUser(
			_discordUserCache,
			_discordUserFactory,
			_authenticationHandlerFactory,
			huokanUserId,
			discordUserId));
	}
}

