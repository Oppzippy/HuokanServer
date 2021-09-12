using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class CreateGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestCreateGuild()
		{
			var repo = new GuildRepository(DbConnection);
			BackedOrganization organization = await CreateOrganization();

			BackedGuild guild = await repo.CreateGuild(new Guild()
			{
				Name = "Bank Guild",
				Realm = "Illidan",
				OrganizationId = organization.Id,
			});
			Assert.NotEqual(Guid.Empty, guild.Id);
			Assert.Equal("Bank Guild", guild.Name);
			Assert.Equal("Illidan", guild.Realm);
			Assert.Equal(organization.Id, guild.OrganizationId);
			Assert.NotEqual(default(DateTime), guild.CreatedAt);
		}
	}
}
