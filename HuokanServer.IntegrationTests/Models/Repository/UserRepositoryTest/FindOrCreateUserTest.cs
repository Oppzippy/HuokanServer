using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class FindOrCreateUserTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestNotFoundCreate()
		{
			BackedUser newlyCreatedUser = await Repository.FindOrCreateUser(new User()
			{
				DiscordUserId = 412,
			});
			Assert.NotNull(newlyCreatedUser);
		}

		[Fact]
		public async Task TestFound()
		{
			BackedUser newlyCreatedUser = await Repository.CreateUser(new User()
			{
				DiscordUserId = 100,
			});
			BackedUser user = await Repository.FindOrCreateUser(new User()
			{
				DiscordUserId = 100,
			});
			Assert.Equal(newlyCreatedUser, user);
		}
	}
}
