using System;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.IntegrationTests.TestBases;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HuokanServer.EndToEndTests.TestBases
{
	public class HttpTestBase : DatabaseTestBase, IDisposable
	{
		public string BaseUrl
		{
			get
			{
				return "http://localhost:5000";
			}
		}

		private readonly IHost _host;

		public HttpTestBase() : base()
		{
			_host = Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.Build();
			Task.WaitAll(_host.StartAsync());
		}

		public void Dispose()
		{
			Task.WaitAll(_host.StopAsync());
			_host.Dispose();
		}

		public async Task<(HttpClient httpClient, BackedUser user)> GetHttpClient()
		{
			var userRepository = new UserRepository(ConnectionFactory);
			var apiKeyRepository = new ApiKeyRepository(ConnectionFactory);
			BackedUser user = await userRepository.CreateUser(new User()
			{
				DiscordUserId = 1,
			});

			string apiKey = await apiKeyRepository.CreateApiKey(new ApiKey()
			{
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(1),
			});

			var client = new HttpClient();
			client.SetBearerToken(apiKey);

			return (client, user);
		}

		public async Task<(HttpClient httpClient, BackedUser user)> GetAdminHttpClient()
		{
			(HttpClient client, BackedUser user) = await GetHttpClient();
			var globalUserPermissionRepository = new GlobalUserPermissionRepository(ConnectionFactory);
			await globalUserPermissionRepository.SetIsAdministrator(user.Id, true);

			return (client, user);
		}
	}
}
