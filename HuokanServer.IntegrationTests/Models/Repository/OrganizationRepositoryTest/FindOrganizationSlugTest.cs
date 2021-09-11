using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class FindOrganizationSlugTest : DatabaseTestBase
	{
		[Fact]
		public async Task TestFindOrganizationSlug()
		{
			var repo = new OrganizationRepository(DbConnection);
			await repo.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization-slug",
			});
			BackedOrganization org = await repo.FindOrganization("organization-slug");
			Assert.NotNull(org);
		}

		[Fact]
		public async Task TestFindNonexistentOrganizationSlug()
		{
			OrganizationRepository repo = new OrganizationRepository(DbConnection);
			await Assert.ThrowsAnyAsync<NotFoundException>(() => repo.FindOrganization("nonexistent-slug"));
		}
	}
}
