using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.OAuth2;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using IdentityModel.Client;

namespace HuokanServer.DataAccess.Discord.User;

public class DiscordUserAuthenticationHandler : IDiscordUserAuthenticationHandler
{
	private readonly IOAuth2 _oAuth2;
	private readonly IUserDiscordTokenRepository _userDiscordTokenRepository;
	private readonly Guid _userId;

	public DiscordUserAuthenticationHandler(IOAuth2 oAuth2, IUserDiscordTokenRepository userDiscordTokenRepository, Guid userId)
	{
		_oAuth2 = oAuth2;
		_userDiscordTokenRepository = userDiscordTokenRepository;
		_userId = userId;
	}

	public async Task<string> GetToken()
	{
		UserDiscordToken token = await _userDiscordTokenRepository.GetDiscordToken(_userId);
		return await RefreshTokenIfExpired(token);
	}

	private async Task<string> RefreshTokenIfExpired(UserDiscordToken token)
	{
		if (token.ExpiresAt <= DateTimeOffset.UtcNow)
		{
			return await ForceRefreshToken(token);
		}
		return token.Token;
	}

	public async Task<string> ForceRefreshToken()
	{
		UserDiscordToken token = await _userDiscordTokenRepository.GetDiscordToken(_userId);
		return await ForceRefreshToken(token);
	}

	private async Task<string> ForceRefreshToken(UserDiscordToken token)
	{
		TokenResponse response = await _oAuth2.RefreshToken(token.RefreshToken);
		token = token with
		{
			Token = response.AccessToken,
			RefreshToken = response.RefreshToken,
			ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn),
		};
		await _userDiscordTokenRepository.SetDiscordToken(_userId, token);
		return token.Token;
	}
}
