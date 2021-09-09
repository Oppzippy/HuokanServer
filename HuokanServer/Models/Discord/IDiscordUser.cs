using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace HuokanServer.Models.Discord
{
	public interface IDiscordUser
	{
		ulong Id { get; }
		List<ulong> GuildIds { get; }
		Task<DiscordMember> GuildMember(ulong guildId);
	}
}
