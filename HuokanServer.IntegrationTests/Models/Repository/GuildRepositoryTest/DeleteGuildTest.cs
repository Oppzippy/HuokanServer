using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class DeleteGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestDeleteGuild()
		{
			var repo = new GuildRepository(DbConnection);
			BackedOrganization organization = await CreateOrganization();
			BackedGuild guild = await repo.CreateGuild(new Guild()
			{
				Name = "Test guild",
				Realm = "Bleeding Hollow",
				OrganizationId = organization.Id,
			});
			await repo.DeleteGuild(guild);
			await Assert.ThrowsAnyAsync<NotFoundException>(() => repo.GetGuild(organization.Id, guild.Id));
		}
	}
}
