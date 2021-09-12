using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class GetNonexistentGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestGetWithNonexistentOrganizationAndGuild()
		{
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.GetGuild(Guid.Empty, Guid.Empty));
		}

		[Fact]
		public async Task TestGetWithNonexistentGuild()
		{
			BackedOrganization organization = await CreateOrganization();
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.GetGuild(organization.Id, Guid.Empty));
		}
	}
}
