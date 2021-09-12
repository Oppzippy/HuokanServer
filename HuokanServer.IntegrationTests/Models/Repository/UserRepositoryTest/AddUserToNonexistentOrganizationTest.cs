using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class AddUserToNonexistentOrganizationTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestAddUserToNonexistentOrganization()
		{
			BackedUser user = await Repository.CreateUser(new User()
			{
				DiscordUserId = 1,
			});

			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.AddUserToOrganization(user.Id, Guid.Empty));
		}
	}
}
