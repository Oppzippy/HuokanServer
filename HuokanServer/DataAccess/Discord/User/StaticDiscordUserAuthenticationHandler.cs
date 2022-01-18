using System;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Discord.User;

public class StaticDiscordUserAuthenticationHandler : IDiscordUserAuthenticationHandler
{
	private readonly string _token;

	public StaticDiscordUserAuthenticationHandler(string token)
	{
		_token = token;
	}

	public Task<string> ForceRefreshToken()
	{
		// TODO use different exception type
		throw new NotImplementedException();
	}

	public Task<string> GetToken()
	{
		return Task.FromResult(_token);
	}
}
