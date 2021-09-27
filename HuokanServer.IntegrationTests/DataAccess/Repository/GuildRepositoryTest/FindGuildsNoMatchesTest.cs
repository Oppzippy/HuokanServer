using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest
{
	public class FindGuildsTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestNoMatches()
		{
			BackedOrganization organization = await CreateOrganization();
			List<BackedGuild> guilds = await Repository.FindGuilds(organization.Id, new GuildFilter() { });
			Assert.Empty(guilds);
		}
	}
}
