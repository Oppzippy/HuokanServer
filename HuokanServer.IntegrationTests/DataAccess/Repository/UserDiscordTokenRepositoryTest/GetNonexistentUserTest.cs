using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserDiscordTokenRepositoryTest;

public class GetNonexistentUserTest : UserDiscordTokenRepositoryTestBase
{
	[Fact]
	public async Task TestGetNonexistentUser()
	{
		await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.GetDiscordToken(Guid.Empty));
	}
}