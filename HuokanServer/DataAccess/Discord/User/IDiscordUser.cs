using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord.User
{
	public interface IDiscordUser
	{
		Task<ulong> GetId();
		Task<List<ulong>> GetGuildIds();
	}
}
