using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class GuildRepositoryTestBase : DatabaseTestBase
	{
		public async Task<BackedOrganization> CreateOrganization()
		{
			var repo = new OrganizationRepository(DbConnection);
			return await repo.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Test Organization",
				Slug = "test-organization"
			});
		}
	}
}
