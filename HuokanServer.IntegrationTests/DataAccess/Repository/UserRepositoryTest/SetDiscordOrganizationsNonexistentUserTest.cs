using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest;

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
