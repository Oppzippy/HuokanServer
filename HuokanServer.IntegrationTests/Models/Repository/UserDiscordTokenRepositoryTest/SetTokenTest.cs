using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.UserDiscordTokenRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserDiscordTokenRepositoryTest
{
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
				ExpiresAt = DateTime.UtcNow.AddDays(1),
			});
		}
	}
}