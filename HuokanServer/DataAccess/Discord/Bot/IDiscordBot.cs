using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord.Bot
{
	public interface IDiscordBot
	{
		Task Connect();
		Task Disconnect();
		Task<DiscordGuildMember> GetGuildMember(ulong guildId, ulong userId);
	}
}
