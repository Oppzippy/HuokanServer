using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord.User
{
	public interface IDiscordUser
	{
		ulong Id { get; }
		Task<List<ulong>> GetGuildIds();
	}
}
