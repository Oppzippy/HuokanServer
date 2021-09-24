using System;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace HuokanServer.Web.Middleware
{
	public class ApiKeyAuthenticationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IApiKeyRepository _apiKeyRepository;
		private readonly IUserRepository _userRepository;

		public ApiKeyAuthenticationMiddleware(RequestDelegate next, IApiKeyRepository apiKeyRepository, IUserRepository userRepository)
		{
			_next = next;
			_apiKeyRepository = apiKeyRepository;
			_userRepository = userRepository;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Headers.TryGetValue("X-API-Key", out StringValues apiKey))
			{
				try
				{
					BackedApiKey apiKeyInfo = await _apiKeyRepository.FindApiKey(apiKey.First());
					BackedUser user = await _userRepository.GetUser(apiKeyInfo.UserId);
					context.Features.Set<BackedUser>(user);
					context.Features.Set<BackedApiKey>(apiKeyInfo);
				}
				catch (InvalidOperationException) { } // API key header is set with no values
				catch (ItemNotFoundException) { } // API key or user not found
			}
			await _next(context);
		}
	}
}
