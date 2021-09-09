using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.UserDiscordTokenRepository;

namespace HuokanServer.Models.Discord
{
	public interface IDiscordUserAuthenticationHandler
	{
		Task<string> ForceRefreshToken(Guid userId, UserDiscordToken token);
		Task<string> GetToken(Guid userId);
	}
}
