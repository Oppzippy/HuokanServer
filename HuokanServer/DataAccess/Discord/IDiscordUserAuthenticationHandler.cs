using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;

namespace HuokanServer.DataAccess.Discord
{
	public interface IDiscordUserAuthenticationHandler
	{
		Task<string> ForceRefreshToken(Guid userId);
		Task<string> GetToken(Guid userId);
	}
}
