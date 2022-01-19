using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserDiscordTokenRepositoryTest;

public class SetTokenNonexistentUserTest : UserDiscordTokenRepositoryTestBase
{
	[Fact]
	public async Task TestSetNonexistentUser()
	{
		await Assert.ThrowsAnyAsync<ItemNotFoundException>(() =>
			Repository.SetDiscordToken(Guid.Empty, new UserDiscordToken()
			{
				Token = "token",
				RefreshToken = "refresh",
				ExpiresAt = DateTimeOffset.UtcNow.AddDays(1),
			})
		);
	}
}
