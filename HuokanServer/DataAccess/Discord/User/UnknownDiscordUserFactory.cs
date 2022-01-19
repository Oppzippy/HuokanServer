using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord.User;

public class UnknownDiscordUserFactory : IUnknownDiscordUserFactory
{
	private readonly IDiscordUserAuthenticationHandlerFactory _authenticationHandlerFactory;

	public UnknownDiscordUserFactory(IDiscordUserAuthenticationHandlerFactory discordUserAuthenticationHandler)
	{
		_authenticationHandlerFactory = discordUserAuthenticationHandler;
	}

	public Task<IDiscordUser> Create(string token)
	{
		IDiscordUserAuthenticationHandler authenticationHandler = _authenticationHandlerFactory.Create(token);
		return Task.FromResult<IDiscordUser>(new DiscordUser(authenticationHandler, token));
	}
}
