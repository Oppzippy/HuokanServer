using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class FindGuildsFilterNameTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestNameFilter()
		{
			var repo = new GuildRepository(DbConnection);
			BackedOrganization organization = await CreateOrganization();
			BackedGuild[] newlyCreatedGuilds = await Task.WhenAll(
				repo.CreateGuild(new Guild()
				{
					Name = "Guild one",
					Realm = "Realm one",
					OrganizationId = organization.Id,
				}),
				repo.CreateGuild(new Guild()
				{
					Name = "Guild two",
					Realm = "Relam two",
					OrganizationId = organization.Id,
				})
			);
			List<BackedGuild> guilds = await repo.FindGuilds(organization.Id, new GuildFilter()
			{
				Name = "Guild two",
			});

			Assert.Equal(1, guilds.Count);
			Assert.Equal(newlyCreatedGuilds[1], guilds[0]);
		}
	}
}
