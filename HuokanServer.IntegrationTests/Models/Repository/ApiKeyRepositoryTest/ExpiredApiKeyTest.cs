using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.ApiKeyRepositoryTest
{
	public class ExpiredApiKeyTest : ApiKeyRepositoryTestBase
	{
		[Fact]
		public async Task TestExpiredApiKeyShouldNotBeFound()
		{
			BackedUser user = await CreateUser();
			string key = await Repository.CreateApiKey(new ApiKey()
			{
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddMinutes(-1),
			});
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.FindApiKey(key));
		}
	}
}
