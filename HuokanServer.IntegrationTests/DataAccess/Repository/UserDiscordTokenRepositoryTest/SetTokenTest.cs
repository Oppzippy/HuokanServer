using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserDiscordTokenRepositoryTest;

public class SetTokenTest : UserDiscordTokenRepositoryTestBase
{
	[Fact]
	public async Task TestSet()
	{
		BackedUser user = await CreateUser();
		await Repository.SetDiscordToken(user.Id, new UserDiscordToken()
		{
			Token = "token",
			RefreshToken = "refresh",
			ExpiresAt = DateTimeOffset.UtcNow.AddDays(1),
		});
	}
}