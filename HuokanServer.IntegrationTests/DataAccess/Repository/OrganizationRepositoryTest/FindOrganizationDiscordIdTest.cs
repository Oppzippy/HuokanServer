using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.IntegrationTests.TestBases;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.OrganizationRepositoryTest;

public class FindOrganizationDiscordIdTest : OrganizationRepositoryTestBase
{
	[Fact]
	public async Task TestFindOrganizaitonByDiscordId()
	{
		await Repository.CreateOrganization(new Organization()
		{
			DiscordGuildId = 123,
			Name = "test",
			Slug = "test",
		});
		BackedOrganization org = await Repository.FindOrganization(123);
		Assert.NotNull(org);
	}

	[Fact]
	public async Task TestFindWithNonexistentDiscordId()
	{
		await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.FindOrganization(1));
	}
}