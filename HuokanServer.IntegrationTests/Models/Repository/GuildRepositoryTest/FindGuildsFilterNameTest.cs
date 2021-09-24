using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class FindGuildsFilterNameTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestNameFilter()
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
				Name = "Guild two",
			});

			Assert.Single(guilds);
			Assert.Equal(newlyCreatedGuilds[1], guilds[0]);
		}
	}
}
