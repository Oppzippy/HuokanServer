using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest;

public class SetDiscordOrganizationsTest : UserRepositoryTestBase
{
	[Fact]
	public async Task TestSetDiscordOrganizations()
	{
		var organizationRepository = new OrganizationRepository(ConnectionFactory);
		BackedOrganization[] organizations = await Task.WhenAll(
			organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization one",
				Slug = "organization-one",
			}),
			organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 2,
				Name = "Organization two",
				Slug = "organization-two"
			}),
			organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 3,
				Name = "Organization three",
				Slug = "organization-three"
			})
		);

		BackedUser user = await Repository.CreateUser(new User()
		{
			DiscordUserId = 1,
		});
		await Repository.SetDiscordOrganizations(user.Id, new List<ulong>() {
			organizations[0].DiscordGuildId,
			organizations[1].DiscordGuildId
		});
		Assert.Equal(
			new HashSet<BackedOrganization>(organizations.Take(2)),
			new HashSet<BackedOrganization>(await organizationRepository.FindOrganizationsContainingUser(user.Id))
		);
	}
}
