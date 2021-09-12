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
			var repo = new GuildRepository(DbConnection);
			await Assert.ThrowsAnyAsync<NotFoundException>(() => repo.GetGuild(Guid.Empty, Guid.Empty));
		}

		[Fact]
		public async Task TestGetNonexistentGuild()
		{
			var repo = new GuildRepository(DbConnection);
			BackedOrganization organization = await CreateOrganization();
			await Assert.ThrowsAnyAsync<NotFoundException>(() => repo.GetGuild(organization.Id, Guid.Empty));
		}
	}
}
