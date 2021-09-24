using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class AddNonexistentUserToOrganizationTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestAddNonexistentUserToOrganization()
		{
			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			BackedOrganization organization = await organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization",
			});

			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.AddUserToOrganization(Guid.Empty, organization.Id));
		}
	}
}
