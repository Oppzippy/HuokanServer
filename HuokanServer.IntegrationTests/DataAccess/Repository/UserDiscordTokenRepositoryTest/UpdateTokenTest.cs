using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserDiscordTokenRepositoryTest;

public class UpdateTokenTest : UserDiscordTokenRepositoryTestBase
{
	[Fact]
	public async Task TestUpdate()
	{
		BackedUser user = await CreateUser();
		await Repository.SetDiscordToken(user.Id, new UserDiscordToken()
		{
			Token = "token",
			RefreshToken = "refresh",
			ExpiresAt = DateTimeOffset.UtcNow.AddDays(1),
		});
		DateTimeOffset secondExpiration = DateTimeOffset.UtcNow.AddDays(2);
		await Repository.SetDiscordToken(user.Id, new UserDiscordToken()
		{
			Token = "token2",
			RefreshToken = "refresh2",
			ExpiresAt = secondExpiration,
		});

		BackedUserDiscordToken token = await Repository.GetDiscordToken(user.Id);
		Assert.Equal("token2", token.Token);
		Assert.Equal("refresh2", token.RefreshToken);
		// Some precision is lost in the db so they won't be exactly equal
		Assert.True(secondExpiration.Subtract(token.ExpiresAt).Duration() < TimeSpan.FromSeconds(1));
	}
}
