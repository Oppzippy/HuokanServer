using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.DepositRepositoryTest;

public class GetDepositTest : DepositRepositoryTestBase
{
	[Fact]
	public async Task TestGetDeposit()
	{
		BackedGuild guild = await CreateGuild();
		BackedUser user = await CreateUser(guild.OrganizationId);

		await Repository.Import(guild.OrganizationId, guild.Id, user.Id, new List<Deposit>()
		{
			new()
			{
				CharacterName = "Name",
				CharacterRealm = "Realm",
				DepositInCopper = 1,
				GuildBankCopper = 1,
			},
		});

		List<BackedDeposit> deposits = await Repository.GetDeposits(guild.OrganizationId, guild.Id);
		BackedDeposit deposit = await Repository.GetDeposit(guild.OrganizationId, guild.Id, deposits.First().Id);
		Assert.Equal(deposits.First(), deposit);
	}
}
