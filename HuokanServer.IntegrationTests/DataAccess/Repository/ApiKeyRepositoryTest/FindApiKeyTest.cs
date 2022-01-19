using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.ApiKeyRepositoryTest;

public class FindApiKeyTest : ApiKeyRepositoryTestBase
{
	[Fact]
	public async Task TestFind()
	{
		BackedUser user = await CreateUser();
		string newlyCreatedKey = await Repository.CreateApiKey(new ApiKey()
		{
			UserId = user.Id,
			ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(1),
		});
		BackedApiKey apiKey = await Repository.FindApiKey(newlyCreatedKey);
		Assert.NotNull(apiKey);
	}
}