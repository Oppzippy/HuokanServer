using System;
using HuokanServer.DataAccess.OAuth2;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;

namespace HuokanServer.DataAccess.Discord.User
{
	public class DiscordUserAuthenticationHandlerFactory : IDiscordUserAuthenticationHandlerFactory
	{
		private readonly ApplicationSettings _settings;
		private readonly IOAuth2Factory _oAuth2Factory;
		private readonly IUserDiscordTokenRepository _userDiscordTokenRepository;

		public DiscordUserAuthenticationHandlerFactory(ApplicationSettings settings, IOAuth2Factory oAuth2Factory, IUserDiscordTokenRepository userDiscordTokenRepository)
		{
			_settings = settings;
			_oAuth2Factory = oAuth2Factory;
			_userDiscordTokenRepository = userDiscordTokenRepository;
		}

		public IDiscordUserAuthenticationHandler Create(Guid userId)
		{
			IOAuth2 oAuth2 = _oAuth2Factory.CreateDiscord(_settings.DiscordClientId, _settings.DiscordClientSecret);
			return new DiscordUserAuthenticationHandler(oAuth2, _userDiscordTokenRepository, userId);
		}

		public IDiscordUserAuthenticationHandler Create(string token)
		{
			IOAuth2 oAuth2 = _oAuth2Factory.CreateDiscord(_settings.DiscordClientId, _settings.DiscordClientSecret);
			return new StaticDiscordUserAuthenticationHandler(token);
		}
	}
}
