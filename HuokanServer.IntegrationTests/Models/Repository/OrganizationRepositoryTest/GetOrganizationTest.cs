using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class GetOrganizationTest : DatabaseTestBase
	{
		[Fact]
		public async Task TestGetOrganization()
		{
			var repo = new OrganizationRepository(DbConnection);
			BackedOrganization createdOrg = await repo.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization"
			});
			BackedOrganization org = await repo.GetOrganization(createdOrg.Id);
			Assert.Equal(createdOrg, org);
		}
	}
}
