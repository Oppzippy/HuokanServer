using System;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.EndToEndTests.Mocks;
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
				return "http://localhost:53234";
			}
		}

		private readonly IHost _host;

		public HttpTestBase() : base()
		{
			_host = Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
					webBuilder.UseUrls(BaseUrl);
					webBuilder.ConfigureServices(services =>
					{
						services.AddMocks();
					});
				})
				.Build();
			Task.WaitAll(_host.StartAsync());
		}

		public void Dispose()
		{
			Task.WaitAll(_host.StopAsync());
			_host.Dispose();
		}

		public async Task<HttpClient> GetHttpClient(BackedUser user)
		{
			var apiKeyRepository = new ApiKeyRepository(ConnectionFactory);
			string apiKey = await apiKeyRepository.CreateApiKey(new ApiKey()
			{
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(1),
			});

			var client = new HttpClient();
			client.SetBearerToken(apiKey);

			return client;
		}

		public async Task<BackedUser> CreateUser(ulong id = 1)
		{
			var userRepository = new UserRepository(ConnectionFactory);
			BackedUser user = await userRepository.CreateUser(new User()
			{
				DiscordUserId = id,
			});
			return user;
		}

		public async Task<BackedUser> CreateAdminUser(ulong id = 2)
		{
			BackedUser user = await CreateUser(id);
			var globalUserPermissionRepository = new GlobalUserPermissionRepository(ConnectionFactory);
			await globalUserPermissionRepository.SetIsAdministrator(user.Id, true);
			return user;
		}
	}
}
