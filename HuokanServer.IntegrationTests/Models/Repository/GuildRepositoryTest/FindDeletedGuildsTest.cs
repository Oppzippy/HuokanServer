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
			BackedOrganization organization = await CreateOrganization();
			BackedGuild[] newlyCreatedGuilds = await Task.WhenAll(
				Repository.CreateGuild(new Guild()
				{
					Name = "Guild one",
					Realm = "Realm",
					OrganizationId = organization.Id,
				}),
				Repository.CreateGuild(new Guild()
				{
					Name = "Guild two",
					Realm = "Realm",
					OrganizationId = organization.Id,
				})
			);
			await Repository.DeleteGuild(organization.Id, newlyCreatedGuilds[1].Id);
			List<BackedGuild> guilds = await Repository.FindGuilds(organization.Id, new GuildFilter() { });
			Assert.Single(guilds);
			Assert.Equal(newlyCreatedGuilds[0], guilds[0]);
		}
	}
}
