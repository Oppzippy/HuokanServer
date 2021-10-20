using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.ApiKeyRepositoryTest
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
				ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
			});
			Assert.NotNull(apiKey);
		}
	}
}
