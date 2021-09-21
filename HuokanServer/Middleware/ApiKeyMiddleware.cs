using System.Linq;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace HuokanServer.Middleware
{
	public class ApiKeyMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IApiKeyRepository _apiKeyRepository;
		private readonly IUserRepository _userRepository;

		public ApiKeyMiddleware(RequestDelegate next, IApiKeyRepository apiKeyRepository, IUserRepository userRepository)
		{
			_next = next;
			_apiKeyRepository = apiKeyRepository;
			_userRepository = userRepository;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			StringValues apiKey;
			if (context.Request.Headers.TryGetValue("X-API-Key", out apiKey))
			{
				try
				{
					BackedApiKey apiKeyInfo = await _apiKeyRepository.FindApiKey(apiKey.First());
					BackedUser user = await _userRepository.GetUser(apiKeyInfo.UserId);
					context.Features.Set<BackedUser>(user);
					context.Features.Set<BackedApiKey>(apiKeyInfo);
				}
				catch (ItemNotFoundException) { }
			}
			await _next(context);
		}

		private record ApiKeyErrorResponse
		{
			public string Error { get; init; }
		}
	}
}
