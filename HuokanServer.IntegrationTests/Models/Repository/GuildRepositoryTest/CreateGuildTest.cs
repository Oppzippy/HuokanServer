using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class CreateGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestCreate()
		{
			BackedOrganization organization = await CreateOrganization();

			BackedGuild guild = await Repository.CreateGuild(new Guild()
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
