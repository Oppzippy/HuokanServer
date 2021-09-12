using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class FindDeletedGuildsTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestDoesntFindDeletedGuilds()
		{
			var repo = new GuildRepository(DbConnection);
			BackedOrganization organization = await CreateOrganization();
			BackedGuild[] newlyCreatedGuilds = await Task.WhenAll(
				repo.CreateGuild(new Guild()
				{
					Name = "Guild one",
					Realm = "Realm",
					OrganizationId = organization.Id,
				}),
				repo.CreateGuild(new Guild()
				{
					Name = "Guild two",
					Realm = "Realm",
					OrganizationId = organization.Id,
				})
			);
			await repo.DeleteGuild(organization.Id, newlyCreatedGuilds[1].Id);
			List<BackedGuild> guilds = await repo.FindGuilds(organization.Id, new GuildFilter() { });
			Assert.Equal(1, guilds.Count);
			Assert.Equal(newlyCreatedGuilds[0], guilds[0]);
		}
	}
}
