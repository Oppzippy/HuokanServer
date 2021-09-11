using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class FindOrganizationDiscordIdTest : DatabaseTestBase
	{
		[Fact]
		public async Task TestFindOrganizaitonDiscordId()
		{
			var repo = new OrganizationRepository(DbConnection);
			await repo.CreateOrganization(new Organization()
			{
				DiscordGuildId = 123,
				Name = "test",
				Slug = "test",
			});
			BackedOrganization org = await repo.FindOrganization(123);
			Assert.NotNull(org);
		}

		[Fact]
		public async Task TestFindNonexistentOrganizationDiscordId()
		{
			var repo = new OrganizationRepository(DbConnection);
			await Assert.ThrowsAnyAsync<NotFoundException>(() => repo.FindOrganization(1));
		}
	}
}
