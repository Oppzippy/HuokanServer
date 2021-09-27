using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest
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
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.DeleteGuild(Guid.Empty, guild.Id));
		}
	}
}
