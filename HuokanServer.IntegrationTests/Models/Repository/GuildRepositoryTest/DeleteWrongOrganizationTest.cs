using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class DeleteWrongOrganizationTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestDeleteGuildWithWrongOrganization()
		{
			var repo = new GuildRepository(DbConnection);
			BackedOrganization organization = await CreateOrganization();
			BackedGuild guild = await repo.CreateGuild(new Guild()
			{
				Name = "Test",
				Realm = "Test",
				OrganizationId = organization.Id,
			});
			BackedGuild guildWithWrongOrganizationId = guild with
			{
				OrganizationId = Guid.Empty,
			};
			await Assert.ThrowsAnyAsync<NotFoundException>(() => repo.DeleteGuild(guildWithWrongOrganizationId));
		}
	}
}
