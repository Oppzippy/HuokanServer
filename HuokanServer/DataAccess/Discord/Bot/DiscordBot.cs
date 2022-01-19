using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using HuokanServer.DataAccess.Cache.DiscordGuildMember;

namespace HuokanServer.DataAccess.Discord.Bot;

public class DiscordBot : IDiscordBot
{
	private readonly DiscordClient _client;

	public DiscordBot(DiscordClient client)
	{
		_client = client;
	}

	public async Task<DiscordGuildMember> GetGuildMember(ulong guildId, ulong userId)
	{
		DiscordGuild guild = await _client.GetGuildAsync(guildId);
		DiscordMember member = await guild.GetMemberAsync(userId);

		return DiscordGuildMember.FromDSharpPlus(member);
	}
}
