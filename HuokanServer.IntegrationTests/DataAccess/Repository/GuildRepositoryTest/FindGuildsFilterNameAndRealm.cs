using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest;

public class FindGuildsFilterNameAndRealm : GuildRepositoryTestBase
{
	[Fact]
	public async Task TestNameAndRealmFilter()
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
				Realm = "Realm one",
				OrganizationId = organization.Id,
			}),
			Repository.CreateGuild(new Guild()
			{
				Name = "Guild one",
				Realm = "Realm two",
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
			Realm = "Realm two",
		});

		Assert.Single(guilds);
		Assert.Equal(newlyCreatedGuilds[3], guilds[0]);
	}
}