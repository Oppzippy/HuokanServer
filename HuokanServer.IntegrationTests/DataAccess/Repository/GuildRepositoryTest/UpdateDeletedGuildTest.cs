using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest;

public class UpdateNonexistentGuildTest : GuildRepositoryTestBase
{
	[Fact]
	public async Task TestUpdateDeletedGuild()
	{
		BackedOrganization organization = await CreateOrganization();
		BackedGuild guild = await Repository.CreateGuild(new Guild()
		{
			Name = "Test",
			Realm = "test",
			OrganizationId = organization.Id,
		});
		await Repository.DeleteGuild(organization.Id, guild.Id);
		await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.UpdateGuild(guild));
	}
}
