using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest
{
	public class FindUsersInOrganizationTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestFindUsersInOrganization()
		{
			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			BackedOrganization organization = await organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization",
			});
			BackedUser[] users = await Task.WhenAll(
				Repository.CreateUser(new User()
				{
					DiscordUserId = 1,
				}),
				Repository.CreateUser(new User()
				{
					DiscordUserId = 2,
				}),
				Repository.CreateUser(new User()
				{
					DiscordUserId = 3,
				})
			);

			await Repository.AddUserToOrganization(users[0].Id, organization.Id);
			await Repository.AddUserToOrganization(users[1].Id, organization.Id);
			Assert.Equal(
				new HashSet<BackedUser>(users.Take(2)),
				new HashSet<BackedUser>(await Repository.FindUsersInOrganization(organization.Id))
			);
		}
	}
}
