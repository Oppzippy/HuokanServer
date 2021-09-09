using System;
using System.Threading.Tasks;

namespace HuokanServer.Models.Repository.UserDiscordTokenRepository
{
	public interface IUserDiscordTokenRepository
	{
		Task<UserDiscordToken> GetDiscordToken(Guid userId);
		Task SetDiscordToken(Guid userId, UserDiscordToken token);
	}
}
