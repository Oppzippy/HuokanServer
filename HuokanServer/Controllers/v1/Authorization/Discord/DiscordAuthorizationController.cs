using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Discord;
using HuokanServer.Models.OAuth2;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserDiscordTokenRepository;
using HuokanServer.Models.Repository.UserRepository;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace HuokanServer.Controllers.v1.Authorization.Discord
{
	[ApiController]
	[Route("/authorization/discord/[action]")]
	public class DiscordAuthorizationController : ControllerBase
	{
		private readonly ApplicationSettings _settings;
		private readonly IOAuth2 _oAuthClient;
		private readonly IDiscordUserFactory _discordUserFactory;
		private readonly IUserRepository _userRepository;
		private readonly IApiKeyRepository _apiKeyRepository;
		private readonly IUserDiscordTokenRepository _userDiscordTokenRepository;

		public DiscordAuthorizationController(
			ApplicationSettings settings,
			IOAuth2Factory oAuth2Factory,
			IDiscordUserFactory discordUserFactory,
			IUserRepository userRepository,
			IApiKeyRepository apiKeyRepository,
			IUserDiscordTokenRepository userDiscordTokenRepository
		)
		{
			_settings = settings;
			_oAuthClient = oAuth2Factory.CreateDiscord(settings.DiscordClientId, settings.DiscordClientSecret); // TODO get client id and secret from config
			_discordUserFactory = discordUserFactory;
			_userRepository = userRepository;
			_apiKeyRepository = apiKeyRepository;
			_userDiscordTokenRepository = userDiscordTokenRepository;
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
		public async Task<DiscordAuthorizeResponse> Authorize([FromQuery(Name = "code")] string code)
		{
			TokenResponse token = await _oAuthClient.GetToken(code, "");
			IDiscordUser discordUser = await _discordUserFactory.Create(token.AccessToken);
			BackedUser user = await _userRepository.FindOrCreateUser(new User()
			{
				DiscordUserId = discordUser.Id,
			});
			await _userDiscordTokenRepository.SetDiscordToken(user.Id, new UserDiscordToken()
			{
				Token = token.AccessToken,
				RefreshToken = token.RefreshToken,
				ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn),
			});
			string apiKey = await _apiKeyRepository.CreateApiKey(new ApiKey()
			{
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(7),
				CreatedAt = DateTime.UtcNow,
			});

			return new DiscordAuthorizeResponse()
			{
				ApiKey = apiKey,
			};
		}

		[HttpGet]
		[Authorize(Policy = "User")]
		public async Task JoinOrganizations()
		{
			BackedUser user = HttpContext.Features.Get<BackedUser>();
			IDiscordUser discordUser = await _discordUserFactory.Create(user.Id);
			List<ulong> guildIds = discordUser.GuildIds;
			await _userRepository.SetDiscordOrganizations(user.Id, guildIds);
		}
	}
}
