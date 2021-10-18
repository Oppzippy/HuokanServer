using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace HuokanServer.DataAccess.Discord
{
	public interface IDiscordUser
	{
		ulong Id { get; }
		Task<List<ulong>> GetGuildIds();
		Task<DiscordMember> GuildMember(ulong guildId);
	}
}
