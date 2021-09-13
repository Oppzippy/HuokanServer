using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.ApiKeyRepositoryTest
{
	public class FindApiKeyTest : ApiKeyRepositoryTestBase
	{
		[Fact]
		public async Task TestFind()
		{
			BackedUser user = await CreateUser();
			string newlyCreatedKey = await Repository.CreateApiKey(new ApiKey()
			{
				UserId = user.Id,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddMinutes(1),
			});
			BackedApiKey apiKey = await Repository.FindApiKey(newlyCreatedKey);
			Assert.NotNull(apiKey);
		}
	}
}
