using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.DepositRepositoryTest;

public class FindNoneTest : DepositRepositoryTestBase
{
	[Fact]
	public async Task TestGetNoDeposits()
	{
		BackedGuild guild = await CreateGuild();
		List<BackedDeposit> deposits = await Repository.GetDeposits(guild.OrganizationId, guild.Id);
		Assert.Empty(deposits);
	}
}