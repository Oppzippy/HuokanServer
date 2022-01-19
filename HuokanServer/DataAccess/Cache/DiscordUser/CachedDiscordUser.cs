using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord.User;

namespace HuokanServer.DataAccess.Cache.DiscordUser;

public class CachedDiscordUser : IDiscordUser
{
	private readonly DiscordUserCache _discordUserCache;
	private readonly IUnknownDiscordUserFactory _userFactory;
	private readonly IDiscordUserAuthenticationHandlerFactory _authenticationHandlerFactory;
	private readonly Guid _huokanUserId;
	private readonly ulong _userId;
	private IDiscordUser _fallback;

	public CachedDiscordUser(
		DiscordUserCache discordUserCache,
		IUnknownDiscordUserFactory userFactory,
		IDiscordUserAuthenticationHandlerFactory authenticationHandlerFactory,
		Guid huokanUserId,
		ulong userId)
	{
		_discordUserCache = discordUserCache;
		_userFactory = userFactory;
		_authenticationHandlerFactory = authenticationHandlerFactory;
		_huokanUserId = huokanUserId;
		_userId = userId;
	}

	public Task<ulong> GetId()
	{
		return Task.FromResult(_userId);
	}

	public async Task<List<ulong>> GetGuildIds()
	{
		List<ulong> guildIds = await _discordUserCache.GetGuildIds(await GetId());
		if (guildIds != null) return guildIds;
		
		IDiscordUser fallback = await GetFallback();
		guildIds = await fallback.GetGuildIds();
		await _discordUserCache.SetGuildIds(await GetId(), guildIds);
		
		return guildIds;
	}

	private async Task<IDiscordUser> GetFallback()
	{
		if (_fallback != null) return _fallback;
		IDiscordUserAuthenticationHandler authenticationHandler = _authenticationHandlerFactory.Create(_huokanUserId);
		string token = await authenticationHandler.GetToken();
		_fallback = await _userFactory.Create(token);
		return _fallback;
	}
}
