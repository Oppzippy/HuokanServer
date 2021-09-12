using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class GetUserTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestGetUser()
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
