using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest
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
