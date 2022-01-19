using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest;

public class SetDiscordOrganizationsEmpty : UserRepositoryTestBase
{
	[Fact]
	public async Task TestSetDiscordOrganizationsToEmpty()
	{
		BackedUser user = await Repository.CreateUser(new User()
		{
			DiscordUserId = 1,
		});
		await Repository.SetDiscordOrganizations(user.Id, new List<ulong>());
	}
}
