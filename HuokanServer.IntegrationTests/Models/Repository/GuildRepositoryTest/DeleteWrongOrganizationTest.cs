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
			BackedOrganization organization = await CreateOrganization();
			BackedGuild guild = await Repository.CreateGuild(new Guild()
			{
				Name = "Test",
				Realm = "Test",
				OrganizationId = organization.Id,
			});
			await Assert.ThrowsAnyAsync<NotFoundException>(() => Repository.DeleteGuild(Guid.Empty, guild.Id));
		}
	}
}
