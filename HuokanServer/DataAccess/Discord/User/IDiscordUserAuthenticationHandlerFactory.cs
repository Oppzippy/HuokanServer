using System;

namespace HuokanServer.DataAccess.Discord.User;

public interface IDiscordUserAuthenticationHandlerFactory
{
	IDiscordUserAuthenticationHandler Create(Guid userId);
	IDiscordUserAuthenticationHandler Create(string token);
}
