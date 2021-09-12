using System.Data;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class GuildRepositoryTestBase : DatabaseTestBase
	{
		public IGuildRepository Repository
		{
			get
			{
				return new GuildRepository(ConnectionFactory);
			}
		}

		public async Task<BackedOrganization> CreateOrganization()
		{
			var repo = new OrganizationRepository(ConnectionFactory);
			return await repo.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Test Organization",
				Slug = "test-organization"
			});
		}
	}
}
