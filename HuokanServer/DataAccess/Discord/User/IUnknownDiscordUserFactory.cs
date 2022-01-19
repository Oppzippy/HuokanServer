using System;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord.User;

public interface IUnknownDiscordUserFactory
{
	Task<IDiscordUser> Create(string token);
}