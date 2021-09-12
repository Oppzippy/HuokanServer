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
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.DeleteGuild(Guid.Empty, Guid.Empty));
		}
	}
}
