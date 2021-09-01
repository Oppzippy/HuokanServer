using System.Linq;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace HuokanServer.Middleware
{
	public class ApiKeyMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ApiKeyRepository _apiKeyRepository;
		private readonly UserRepository _userRepository;

		public ApiKeyMiddleware(RequestDelegate next, ApiKeyRepository apiKeyRepository, UserRepository userRepository)
		{
			_next = next;
			_apiKeyRepository = apiKeyRepository;
			_userRepository = userRepository;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			StringValues apiKey;
			if (!context.Request.Headers.TryGetValue("X-API-Key", out apiKey))
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsJsonAsync(new ApiKeyErrorResponse()
				{
					Error = "No API key was provided."
				});
				return;
			}
			BackedApiKey apiKeyInfo = await _apiKeyRepository.FindApiKey(apiKey.First());
			if (apiKeyInfo == null)
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsJsonAsync(new ApiKeyErrorResponse()
				{
					Error = "Invalid API key."
				});
				return;
			}

			BackedUser user = await _userRepository.GetUser(apiKeyInfo.UserId);
			context.Features.Set<BackedUser>(user);
			context.Features.Set<BackedApiKey>(apiKeyInfo);
			await _next(context);
		}

		private record ApiKeyErrorResponse
		{
			public string Error { get; init; }
		}
	}
}
