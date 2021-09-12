using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class FindOrganizationSlugTest : OrganizationRepositoryTestBase
	{
		[Fact]
		public async Task TestFindOrganizationSlug()
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
		public async Task TestFindNonexistentOrganizationSlug()
		{
			await Assert.ThrowsAnyAsync<NotFoundException>(() => Repository.FindOrganization("nonexistent-slug"));
		}
	}
}
