using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class DeleteNonexistentGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestDeleteNonexistentOrganizationAndGuild()
		{
			var repo = new GuildRepository(DbConnection);
			var guild = new BackedGuild()
			{
				Id = Guid.Empty,
				OrganizationId = Guid.Empty,
			};
			await Assert.ThrowsAnyAsync<NotFoundException>(() => repo.DeleteGuild(guild));
		}
	}
}
