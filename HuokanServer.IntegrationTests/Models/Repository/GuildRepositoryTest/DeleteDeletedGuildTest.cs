using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class DeleteDeletedGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestDeleteTwice()
		{
			BackedOrganization organization = await CreateOrganization();
			BackedGuild guild = await Repository.CreateGuild(new Guild()
			{
				Name = "Guild Name",
				Realm = "Realm",
				OrganizationId = organization.Id,
			});
			await Repository.DeleteGuild(organization.Id, guild.Id);
			await Assert.ThrowsAnyAsync<NotFoundException>(() => Repository.DeleteGuild(organization.Id, guild.Id));
		}
	}
}
