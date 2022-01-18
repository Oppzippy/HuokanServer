using System;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord.User;

public interface IKnownDiscordUserFactory
{
	Task<IDiscordUser> Create(Guid huokanUserId, ulong discordUserId);
}
