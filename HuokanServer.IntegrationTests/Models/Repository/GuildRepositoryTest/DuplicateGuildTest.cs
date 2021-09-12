using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class DuplicateGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestCreateDuplicate()
		{
			BackedOrganization organization = await CreateOrganization();
			var guild = new Guild()
			{
				Name = "Guild name",
				Realm = "Guild realm",
				OrganizationId = organization.Id,
			};

			await Repository.CreateGuild(guild);
			await Assert.ThrowsAnyAsync<DuplicateItemException>(() => Repository.CreateGuild(guild));
		}
	}
}
