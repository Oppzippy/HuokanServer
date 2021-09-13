using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.ApiKeyRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.ApiKeyRepositoryTest
{
	public class CreateApiKeyNonexistentUserTest : ApiKeyRepositoryTestBase
	{
		[Fact]
		public async Task TestCreateWithNonexistentUser()
		{
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(
				() => Repository.CreateApiKey(new ApiKey()
				{
					UserId = Guid.Empty,
					CreatedAt = DateTime.UtcNow,
					ExpiresAt = DateTime.UtcNow.AddDays(7),
				})
			);
		}
	}
}
