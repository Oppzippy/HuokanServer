using System;
using System.Threading.Tasks;

namespace HuokanServer.Models.Discord
{
	public interface IDiscordUserFactory
	{
		Task<IDiscordUser> Create(Guid userId);
		Task<IDiscordUser> Create(string token);
	}
}
