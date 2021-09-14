using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.UserDiscordTokenRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserDiscordTokenRepositoryTest
{
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
				ExpiresAt = DateTime.UtcNow.AddDays(1),
			});
			BackedUserDiscordToken token = await Repository.GetDiscordToken(user.Id);
			Assert.Equal("token", token.Token);
			Assert.Equal("refresh", token.RefreshToken);
			Assert.NotEqual(default(DateTime), token.CreatedAt);
			Assert.NotEqual(default(DateTime), token.ExpiresAt);
		}
	}
}
