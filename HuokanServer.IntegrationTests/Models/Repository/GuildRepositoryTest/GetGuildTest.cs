using System.Threading.Tasks;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class GetGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestGetGuild()
		{
			BackedOrganization organization = await CreateOrganization();
			BackedGuild newlyCreatedGuild = await Repository.CreateGuild(new Guild()
			{
				Name = "Bank",
				Realm = "Sargeras",
				OrganizationId = organization.Id,
			});
			BackedGuild guild = await Repository.GetGuild(organization.Id, newlyCreatedGuild.Id);
			Assert.Equal(newlyCreatedGuild, guild);
		}
	}
}
