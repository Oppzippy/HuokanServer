using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace HuokanServer.Authentication
{
	public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
	{

	}

	public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
	{
		private readonly IApiKeyRepository _apiKeyRepository;
		private readonly IUserRepository _userRepository;

		public ApiKeyAuthenticationHandler(
			IOptionsMonitor<ApiKeyAuthenticationOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IUserRepository userRepository,
			IApiKeyRepository apiKeyRepository
		) : base(options, logger, encoder, clock)
		{
			_userRepository = userRepository;
			_apiKeyRepository = apiKeyRepository;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			StringValues apiKey;
			if (Request.Headers.TryGetValue("X-API-Key", out apiKey))
			{
				return await ValidateApiKey(apiKey.First());
			}
			return AuthenticateResult.Fail("No api key was provided.");
		}

		private async Task<AuthenticateResult> ValidateApiKey(string apiKey)
		{
			try
			{
				BackedApiKey apiKeyInfo = await _apiKeyRepository.FindApiKey(apiKey);
				BackedUser user = await _userRepository.GetUser(apiKeyInfo.UserId);
				Context.Features.Set<BackedUser>(user);
				Context.Features.Set<BackedApiKey>(apiKeyInfo);

				var claims = new List<Claim>() {
					new Claim(ClaimTypes.Name, apiKey),
				};
				var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
				return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
			}
			catch (ItemNotFoundException)
			{
				return AuthenticateResult.Fail("The api key is invalid.");
			}
		}
	}
}
