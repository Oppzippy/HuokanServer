using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.DepositRepositoryTest;

public class GetDepositsLimitTest : DepositRepositoryTestBase
{
	[Fact]
	public async Task TestGetDepositsLimit()
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
			new()
			{
				CharacterName = "Name2",
				CharacterRealm = "Realm",
				DepositInCopper = 2,
				GuildBankCopper = 3,
				ApproximateDepositTimestamp = DateTimeOffset.UtcNow,
			},
			new()
			{
				CharacterName = "Name3",
				CharacterRealm = "Realm",
				DepositInCopper = 3,
				GuildBankCopper = 6,
				ApproximateDepositTimestamp = DateTimeOffset.UtcNow,
			},
		});

		List<BackedDeposit> deposits = await Repository.GetDeposits(guild.OrganizationId, guild.Id, 2);
		Assert.Equal(2, deposits.Count);
	}
}
