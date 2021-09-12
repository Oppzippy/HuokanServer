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
		public async Task TestDelete()
		{
			BackedOrganization organization = await CreateOrganization();
			BackedGuild guild = await Repository.CreateGuild(new Guild()
			{
				Name = "Test guild",
				Realm = "Bleeding Hollow",
				OrganizationId = organization.Id,
			});
			await Repository.DeleteGuild(organization.Id, guild.Id);
			await Assert.ThrowsAnyAsync<NotFoundException>(() => Repository.GetGuild(organization.Id, guild.Id));
		}
	}
}
