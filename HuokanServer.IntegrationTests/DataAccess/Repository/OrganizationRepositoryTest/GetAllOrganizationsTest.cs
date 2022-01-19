using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.OrganizationRepositoryTest;

public class GetAllOrganizationsTest : OrganizationRepositoryTestBase
{
	[Fact]
	public async Task TestGetAll()
	{
		BackedOrganization[] createdOrganizations = await Task.WhenAll(
			Repository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization",
			}),
			Repository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 2,
				Name = "Organization 2",
				Slug = "organization-2",
			})
		);
		List<BackedOrganization> fetchedOrganizations = await Repository.GetAllOrganizations();
		Assert.Equal(
			new HashSet<BackedOrganization>(createdOrganizations),
			new HashSet<BackedOrganization>(fetchedOrganizations)
		);
	}
}
