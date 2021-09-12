using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class FindGuildsFilterRealmTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestRealmFilter()
		{
			BackedOrganization organization = await CreateOrganization();
			BackedGuild[] newlyCreatedGuilds = await Task.WhenAll(
				Repository.CreateGuild(new Guild()
				{
					Name = "Guild one",
					Realm = "Realm one",
					OrganizationId = organization.Id,
				}),
				Repository.CreateGuild(new Guild()
				{
					Name = "Guild two",
					Realm = "Realm two",
					OrganizationId = organization.Id,
				})
			);
			List<BackedGuild> guilds = await Repository.FindGuilds(organization.Id, new GuildFilter()
			{
				Realm = "Realm one",
			});

			Assert.Single(guilds);
			Assert.Equal(newlyCreatedGuilds[0], guilds[0]);
		}
	}
}
