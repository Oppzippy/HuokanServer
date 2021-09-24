using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.OAuth2;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using IdentityModel.Client;

namespace HuokanServer.DataAccess.Discord
{
	public class DiscordUserAuthenticationHandler : IDiscordUserAuthenticationHandler
	{
		private readonly IOAuth2 _oAuth2;
		private readonly IUserDiscordTokenRepository _userDiscordTokenRepository;

		public DiscordUserAuthenticationHandler(ApplicationSettings settings, IOAuth2Factory oAuth2Factory, IUserDiscordTokenRepository userDiscordTokenRepository)
		{
			_oAuth2 = oAuth2Factory.CreateDiscord(settings.DiscordClientId, settings.DiscordClientSecret);
			_userDiscordTokenRepository = userDiscordTokenRepository;
		}

		public async Task<string> GetToken(Guid userId)
		{
			UserDiscordToken token = await _userDiscordTokenRepository.GetDiscordToken(userId);
			return await RefreshTokenIfExpired(userId, token);
		}

		private async Task<string> RefreshTokenIfExpired(Guid userId, UserDiscordToken token)
		{
			if (token.ExpiresAt <= DateTime.UtcNow)
			{
				return await ForceRefreshToken(userId, token);
			}
			return token.Token;
		}

		public async Task<string> ForceRefreshToken(Guid userId, UserDiscordToken token)
		{
			TokenResponse response = await _oAuth2.RefreshToken(token.RefreshToken);
			token = token with
			{
				Token = response.AccessToken,
				RefreshToken = response.RefreshToken,
				ExpiresAt = DateTime.UtcNow.AddSeconds(response.ExpiresIn),
			};
			await _userDiscordTokenRepository.SetDiscordToken(userId, token);
			return token.Token;
		}
	}
}
