using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest;

public class GetGuildTest : GuildRepositoryTestBase
{
	[Fact]
	public async Task TestGet()
	{
		BackedOrganization organization = await CreateOrganization();
		BackedGuild newlyCreatedGuild = await Repository.CreateGuild(new Guild()
		{
			Name = "Bank",
			Realm = "Sargeras",
			OrganizationId = organization.Id,
		});
		BackedGuild guild = await Repository.GetGuild(organization.Id, newlyCreatedGuild.Id);
		Assert.Equal(newlyCreatedGuild, guild);
	}
}
