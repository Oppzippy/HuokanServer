using System;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord
{
	public interface IDiscordUserFactory
	{
		Task<IDiscordUser> Create(Guid userId);
		Task<IDiscordUser> Create(string token);
	}
}
