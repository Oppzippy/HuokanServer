using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserDiscordTokenRepositoryTest;

public class GetTokenTest : UserDiscordTokenRepositoryTestBase
{
	[Fact]
	public async Task TestGet()
	{
		BackedUser user = await CreateUser();
		await Repository.SetDiscordToken(user.Id, new UserDiscordToken()
		{
			Token = "token",
			RefreshToken = "refresh",
			ExpiresAt = DateTimeOffset.UtcNow.AddDays(1),
		});
		BackedUserDiscordToken token = await Repository.GetDiscordToken(user.Id);
		Assert.Equal("token", token.Token);
		Assert.Equal("refresh", token.RefreshToken);
		Assert.NotEqual(default(DateTimeOffset), token.CreatedAt);
		Assert.NotEqual(default(DateTimeOffset), token.ExpiresAt);
	}
}
