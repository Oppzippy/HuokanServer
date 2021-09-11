using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class GetUserTest : DatabaseTestBase
	{
		[Fact]
		public async Task TestGetUser()
		{
			var repo = new UserRepository(DbConnection);
			BackedUser newlyCreatedUser = await repo.CreateUser(new User()
			{
				DiscordUserId = 123,
			});
			BackedUser user = await repo.GetUser(newlyCreatedUser.Id);
			Assert.NotNull(user);
		}
	}
}
