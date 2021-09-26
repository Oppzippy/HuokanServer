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
			string apiKey = GetApiKey(context);
			if (apiKey != null)
			{
				try
				{
					BackedApiKey apiKeyInfo = await _apiKeyRepository.FindApiKey(apiKey);
					BackedUser user = await _userRepository.GetUser(apiKeyInfo.UserId);
					context.Features.Set<BackedUser>(user);
					context.Features.Set<BackedApiKey>(apiKeyInfo);
				}
				catch (ItemNotFoundException) { } // API key or user not found
			}
			await _next(context);
		}

		private string GetApiKey(HttpContext context)
		{
			return GetApiKeyFromAuthorizationHeader(context) ?? GetApiKeyFromAuthorizationHeader(context);
		}

		private string GetApiKeyFromXApiKeyHeader(HttpContext context)
		{
			if (context.Request.Headers.TryGetValue("X-API-Key", out StringValues apiKeyValues) && apiKeyValues.Count >= 1)
			{
				return apiKeyValues.First();
			}
			return null;
		}

		private string GetApiKeyFromAuthorizationHeader(HttpContext context)
		{
			if (context.Request.Headers.TryGetValue("Authorization", out StringValues apiKeyValues) && apiKeyValues.Count >= 1)
			{
				string authorization = apiKeyValues.First();
				const string bearerPrefix = "bearer ";
				if (authorization.ToLower().StartsWith(bearerPrefix))
				{
					return authorization.Substring(bearerPrefix.Length).Trim();
				}
			}
			return null;
		}
	}
}
