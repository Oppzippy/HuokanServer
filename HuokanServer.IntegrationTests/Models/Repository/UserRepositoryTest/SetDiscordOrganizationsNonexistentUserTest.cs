using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class SetDiscordOrganizationsNonexistentUserTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task SetDiscordOrganizationsOfNonexistentUser()
		{
			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			BackedOrganization organization = await organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization",
			});

			await Assert.ThrowsAnyAsync<ItemNotFoundException>(
				() => Repository.SetDiscordOrganizations(Guid.Empty, new List<ulong>() { organization.DiscordGuildId })
			);
		}
	}
}
