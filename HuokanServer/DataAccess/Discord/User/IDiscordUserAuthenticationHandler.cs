using System;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord.User;

public interface IDiscordUserAuthenticationHandler
{
	Task<string> ForceRefreshToken();
	Task<string> GetToken();
}
