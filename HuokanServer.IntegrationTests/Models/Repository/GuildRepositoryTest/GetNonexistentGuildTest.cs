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
		public async Task TestGetNonexistentOrganizationAndGuild()
		{
			await Assert.ThrowsAnyAsync<NotFoundException>(() => Repository.GetGuild(Guid.Empty, Guid.Empty));
		}

		[Fact]
		public async Task TestGetNonexistentGuild()
		{
			BackedOrganization organization = await CreateOrganization();
			await Assert.ThrowsAnyAsync<NotFoundException>(() => Repository.GetGuild(organization.Id, Guid.Empty));
		}
	}
}