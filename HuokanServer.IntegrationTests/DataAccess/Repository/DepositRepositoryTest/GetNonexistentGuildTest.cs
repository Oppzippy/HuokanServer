using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.DepositRepositoryTest;

public class GetNonexistentGuildTest : DepositRepositoryTestBase
{
	[Fact]
	public async Task TestGetDepositsFromNonexistentGuild()
	{
		await Assert.ThrowsAnyAsync<ItemNotFoundException>(() =>
			Repository.GetNewerDeposits(Guid.Empty, Guid.Empty, null, 5));
	}
}
