using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserPermissionRepository;

public class IsAdministratorTest : GlobalUserPermissionRepositoryTestBase
{
	[Fact]
	public async Task TestIsNotAdministrator()
	{
		var userRepository = new UserRepository(ConnectionFactory);
		BackedUser user = await userRepository.CreateUser(new User()
		{
			DiscordUserId = 1,
		});
		await Repository.SetIsAdministrator(user.Id, true);
		Assert.True(await Repository.IsAdministrator(user.Id));
	}
}