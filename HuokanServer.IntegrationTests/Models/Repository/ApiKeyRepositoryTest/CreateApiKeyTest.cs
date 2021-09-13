using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.ApiKeyRepositoryTest
{
	public class CreateApiKeyTest : ApiKeyRepositoryTestBase
	{
		[Fact]
		public async Task TestCreate()
		{
			BackedUser user = await CreateUser();
			string apiKey = await Repository.CreateApiKey(new ApiKey()
			{
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(7),
			});
			Assert.NotNull(apiKey);
		}
	}
}
