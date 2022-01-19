using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserPermissionRepository;

public class IsNotAdministratorTest : GlobalUserPermissionRepositoryTestBase
{
	[Fact]
	public async Task TestIsNotAdministrator()
	{
		var userRepository = new UserRepository(ConnectionFactory);
		BackedUser user = await userRepository.CreateUser(new User()
		{
			DiscordUserId = 1,
		});
		Assert.False(await Repository.IsAdministrator(user.Id));
	}
}