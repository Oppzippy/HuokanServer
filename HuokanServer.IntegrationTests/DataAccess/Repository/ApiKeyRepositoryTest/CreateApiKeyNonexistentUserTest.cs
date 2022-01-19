using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.ApiKeyRepositoryTest;

public class CreateApiKeyNonexistentUserTest : ApiKeyRepositoryTestBase
{
	[Fact]
	public async Task TestCreateWithNonexistentUser()
	{
		await Assert.ThrowsAnyAsync<ItemNotFoundException>(
			() => Repository.CreateApiKey(new ApiKey()
			{
				UserId = Guid.Empty,
				ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
			})
		);
	}
}
