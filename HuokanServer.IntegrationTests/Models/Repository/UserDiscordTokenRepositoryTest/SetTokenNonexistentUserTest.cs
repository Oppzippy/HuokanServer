using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.UserDiscordTokenRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserDiscordTokenRepositoryTest
{
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
					ExpiresAt = DateTime.UtcNow.AddDays(1),
				})
			);
		}
	}
}
