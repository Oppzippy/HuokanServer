using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.IntegrationTests.TestBases;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest
{
	public class GetUserTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestGet()
		{
			BackedUser newlyCreatedUser = await Repository.CreateUser(new User()
			{
				DiscordUserId = 123,
			});
			BackedUser user = await Repository.GetUser(newlyCreatedUser.Id);
			Assert.NotNull(user);
		}
	}
}
