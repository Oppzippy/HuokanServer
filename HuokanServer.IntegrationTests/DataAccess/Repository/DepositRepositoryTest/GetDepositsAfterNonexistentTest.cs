using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.DepositRepositoryTest;

public class GetDepositsAfterNonexistentTest : DepositRepositoryTestBase
{
	[Fact]
	public async Task TestGetDepositsAfterNonexistent()
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
				ApproximateDepositTimestamp = DateTimeOffset.UtcNow,
			},
		});

		List<BackedDeposit> deposits = await Repository.GetDepositsAfter(guild.OrganizationId, guild.Id, Guid.Empty);
		Assert.Empty(deposits);
	}
}
