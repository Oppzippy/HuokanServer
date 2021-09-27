using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest
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
