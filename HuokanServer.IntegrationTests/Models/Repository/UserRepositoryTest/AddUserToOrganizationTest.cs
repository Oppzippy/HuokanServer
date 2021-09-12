using System.Threading.Tasks;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class AddUserToOrganizationTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestAddUserToOrganization()
		{
			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			BackedUser user = await Repository.CreateUser(new User()
			{
				DiscordUserId = 1,
			});
			BackedOrganization organization = await organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization",
			});

			await Repository.AddUserToOrganization(user.Id, organization.Id);
			Assert.True(await Repository.IsUserInOrganization(user.Id, organization.Id));
		}
	}
}
