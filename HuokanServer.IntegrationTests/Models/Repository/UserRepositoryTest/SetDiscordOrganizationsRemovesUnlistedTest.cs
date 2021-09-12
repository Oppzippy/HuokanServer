using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class SetDiscordOrganizationsRemovesUnlistedTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestSetDiscordOrganizationsRemovesUnlistedOrganizations()
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
					Slug = "organization-two",
				})
			);
			BackedUser user = await Repository.CreateUser(new User()
			{
				DiscordUserId = 1,
			});

			await Repository.AddUserToOrganization(user.Id, organizations[0].Id);
			await Repository.SetDiscordOrganizations(user.Id, new List<ulong>() { organizations[1].DiscordGuildId });
			Assert.Equal(organizations.TakeLast(1), await organizationRepository.FindOrganizationsContainingUser(user.Id));
		}
	}
}
