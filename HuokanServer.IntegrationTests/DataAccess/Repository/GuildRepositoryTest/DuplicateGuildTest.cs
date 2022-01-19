using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest;

public class DuplicateGuildTest : GuildRepositoryTestBase
{
	[Fact]
	public async Task TestCreateDuplicate()
	{
		BackedOrganization organization = await CreateOrganization();
		var guild = new Guild()
		{
			Name = "Guild name",
			Realm = "Guild realm",
			OrganizationId = organization.Id,
		};

		await Repository.CreateGuild(guild);
		await Assert.ThrowsAnyAsync<DuplicateItemException>(() => Repository.CreateGuild(guild));
	}
}
