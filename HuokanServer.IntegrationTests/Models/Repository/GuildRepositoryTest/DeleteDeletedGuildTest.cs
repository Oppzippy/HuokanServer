using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
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
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.DeleteGuild(organization.Id, guild.Id));
		}
	}
}
