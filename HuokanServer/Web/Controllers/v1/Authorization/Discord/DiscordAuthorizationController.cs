using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord.User;
using HuokanServer.DataAccess.OAuth2;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace HuokanServer.Web.Controllers.v1.Authorization.Discord
{
	[ApiController]
	[Route("authorization/discord")]
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
		[Route("redirect")]
		public RedirectResult RedirectToDiscord([Required][FromQuery(Name = "redirectUrl")] string redirectUrl)
		{
			var queryParams = new Dictionary<string, string>()
			{
				{"client_id", _settings.DiscordClientId},
				{"redirect_uri", redirectUrl},
				{"response_type", "code"},
				{"scope", "identify guilds"}
			};
			string url = QueryHelpers.AddQueryString("https://discord.com/api/oauth2/authorize", queryParams);
			return Redirect(url);
		}

		[HttpGet]
		[Route("authorize")]
		public async Task<ActionResult<AuthorizationModel>> Authorize(
			[Required][FromQuery(Name = "code")] string code,
			[Required][FromQuery(Name = "redirectUrl")] string redirectUrl
		)
		{
			TokenResponse token;
			try
			{
				token = await _oAuthClient.GetToken(code, redirectUrl);
			}
			catch (OAuth2Exception)
			{
				return Unauthorized();
			}
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
			});

			return new AuthorizationModel()
			{
				ApiKey = apiKey,
			};
		}

		[HttpGet]
		[Route("refreshOrganizations")]
		[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.USER)]
		public async Task RefreshOrganizations()
		{
			BackedUser user = HttpContext.Features.Get<BackedUser>();
			IDiscordUser discordUser = await _discordUserFactory.Create(user.Id);
			List<ulong> guildIds = await discordUser.GetGuildIds();
			await _userRepository.SetDiscordOrganizations(user.Id, guildIds);
		}
	}
}
