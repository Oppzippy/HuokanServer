using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.OrganizationRepositoryTest
{
	public class GetOrganizationTest : OrganizationRepositoryTestBase
	{
		[Fact]
		public async Task TestGet()
		{
			BackedOrganization createdOrg = await Repository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization",
			});
			BackedOrganization org = await Repository.GetOrganization(createdOrg.Id);
			Assert.Equal(createdOrg, org);
		}
	}
}
