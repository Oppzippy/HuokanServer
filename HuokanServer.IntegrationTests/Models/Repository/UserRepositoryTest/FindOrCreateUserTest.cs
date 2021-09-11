using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class FindOrCreateUserTest : DatabaseTestBase
	{
		[Fact]
		public async Task TestCreateUser()
		{
			var repo = new UserRepository(DbConnection);
			BackedUser newlyCreatedUser = await repo.FindOrCreateUser(new User()
			{
				DiscordUserId = 412,
			});
			Assert.NotNull(newlyCreatedUser);
		}

		[Fact]
		public async Task TestFindUser()
		{
			var repo = new UserRepository(DbConnection);
			BackedUser newlyCreatedUser = await repo.CreateUser(new User()
			{
				DiscordUserId = 100,
			});
			BackedUser user = await repo.FindOrCreateUser(new User()
			{
				DiscordUserId = 100,
			});
			Assert.Equal(newlyCreatedUser, user);
		}
	}
}
