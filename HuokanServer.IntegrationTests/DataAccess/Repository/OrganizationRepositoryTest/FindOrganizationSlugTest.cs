using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.IntegrationTests.TestBases;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.OrganizationRepositoryTest;

public class FindOrganizationSlugTest : OrganizationRepositoryTestBase
{
	[Fact]
	public async Task TestFindSlug()
	{
		await Repository.CreateOrganization(new Organization()
		{
			DiscordGuildId = 1,
			Name = "Organization",
			Slug = "organization-slug",
		});
		BackedOrganization org = await Repository.FindOrganization("organization-slug");
		Assert.NotNull(org);
	}

	[Fact]
	public async Task TestFindNonexistentSlug()
	{
		await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.FindOrganization("nonexistent-slug"));
	}
}