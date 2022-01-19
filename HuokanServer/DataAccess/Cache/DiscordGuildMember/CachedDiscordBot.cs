using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord.Bot;

namespace HuokanServer.DataAccess.Cache.DiscordGuildMember;

public class CachedDiscordBot : IDiscordBot
{
	private readonly IDiscordBot _fallback;
	private readonly DiscordGuildMemberCache _cache;

	public CachedDiscordBot(DiscordGuildMemberCache cache, IDiscordBot fallback)
	{
		_cache = cache;
		_fallback = fallback;
	}

	public async Task<Discord.Bot.DiscordGuildMember> GetGuildMember(ulong guildId, ulong userId)
	{
		Discord.Bot.DiscordGuildMember cachedMember = await _cache.GetGuildMember(guildId, userId);
		if (cachedMember != null) return cachedMember;

		Discord.Bot.DiscordGuildMember member = await _fallback.GetGuildMember(guildId, userId);
		await _cache.SetGuildMember(guildId, userId, member);
		return member;
	}
}
