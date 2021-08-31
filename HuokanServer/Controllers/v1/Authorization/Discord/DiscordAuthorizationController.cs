using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Discord;
using HuokanServer.Models.OAuth2;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserRepository;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace HuokanServer.Controllers.Auth
{
	[ApiController]
	[Route("/authorization/discord/[action]")]
	public class DiscordAuthorizationController : ControllerBase
	{
		private readonly ApplicationSettings _settings;
		private readonly IOAuth2 _oAuthClient;
		private readonly IDiscordUserFactory _discordUserFactory;
		private readonly UserRepository _userRepository;
		private readonly ApiKeyRepository _apiKeyRepository;

		public DiscordAuthorizationController(
			ApplicationSettings settings,
			IOAuth2Factory oAuth2Factory,
			IDiscordUserFactory discordUserFactory,
			UserRepository userRepository,
			ApiKeyRepository apiKeyRepository
		)
		{
			_settings = settings;
			_oAuthClient = oAuth2Factory.CreateDiscord("", ""); // TODO get client id and secret from config
			_discordUserFactory = discordUserFactory;
			_userRepository = userRepository;
			_apiKeyRepository = apiKeyRepository;
		}

		[HttpGet]
		public RedirectResult Redirects([FromQuery(Name = "organization")] string organization)
		{
			var queryParams = new Dictionary<string, string>()
			{
				{"client_id", _settings.DiscordClientId},
				{"redirect_uri", _settings.DiscordRedirectUri},
				{"response_type", "code"},
				{"scope", "identify guilds"}
			};
			string url = QueryHelpers.AddQueryString("https://discord.com/api/oauth2/authorize", queryParams);
			return Redirect(url);
		}

		[HttpGet]
		public async Task<AuthorizeResponse> Authorize([FromQuery(Name = "code")] string code)
		{
			TokenResponse token = await _oAuthClient.GetToken(code, "");
			IDiscordUser discordUser = _discordUserFactory.Create(token.AccessToken);
			BackedUser user = await _userRepository.FindOrCreateUser(new User()
			{
				DiscordUserId = discordUser.Id,
			});
			var userWithNewToken = user with
			{
				DiscordToken = token.AccessToken,
			};
			await _userRepository.UpdateDiscordToken(userWithNewToken);
			string apiKey = await _apiKeyRepository.CreateApiKey(new ApiKey()
			{
				UserId = userWithNewToken.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(7),
				CreatedAt = DateTime.UtcNow,
			});

			return new AuthorizeResponse()
			{
				ApiKey = apiKey,
			};
		}

		public record AuthorizeResponse
		{
			public string ApiKey { get; init; }
		}
	}
}
