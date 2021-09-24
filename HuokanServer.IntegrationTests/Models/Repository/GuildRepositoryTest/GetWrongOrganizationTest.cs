using System;
using System.Data.Common;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class GetWrongOrganizationTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestGetWithWrongOrganizationId()
		{
			BackedOrganization organization = await CreateOrganization();
			BackedGuild guild = await Repository.CreateGuild(new Guild()
			{
				Name = "Guild",
				Realm = "Realm",
				OrganizationId = organization.Id,
			});
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.GetGuild(Guid.Empty, guild.Id));
		}
	}
}
