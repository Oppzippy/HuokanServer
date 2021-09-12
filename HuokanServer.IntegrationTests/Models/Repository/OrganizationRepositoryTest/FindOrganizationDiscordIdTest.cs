using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
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
			await Assert.ThrowsAnyAsync<NotFoundException>(() => Repository.FindOrganization(1));
		}
	}
}
