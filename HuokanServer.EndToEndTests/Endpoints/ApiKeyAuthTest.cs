using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints
{
	public class ApiKeyAuthTest : HttpTestBase
	{
		private string _apiKey;

		public ApiKeyAuthTest() : base()
		{
			Initialize().Wait();
		}

		private async Task Initialize()
		{
			BackedUser user = await CreateUser();
			var apiKeyRepository = new ApiKeyRepository(ConnectionFactory);
			_apiKey = await apiKeyRepository.CreateApiKey(new ApiKey()
			{
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(1),
			});
		}

		[Fact]
		public async Task TestXApiKeyAuth()
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
			HttpResponseMessage response = await httpClient.GetAsync($"{BaseUrl}/users/me");
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task TestBearerTokenAuth()
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
			HttpResponseMessage response = await httpClient.GetAsync($"{BaseUrl}/users/me");
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
	}
}
