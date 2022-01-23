﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.DepositRepositoryTest;

public class GetDepositBeforeTest:DepositRepositoryTestBase
{
	[Fact]
	public async Task TestGetDepositBefore()
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
			new()
			{
				CharacterName = "Name2",
				CharacterRealm = "Realm",
				DepositInCopper = 2,
				GuildBankCopper = 3,
			},
			new()
			{
				CharacterName = "Name3",
				CharacterRealm = "Realm",
				DepositInCopper = 3,
				GuildBankCopper = 6,
			},
		});

		List<BackedDeposit> deposits = await Repository.GetDeposits(guild.OrganizationId, guild.Id);
		BackedDeposit deposit = await Repository.GetDeposit(guild.OrganizationId, guild.Id, deposits.Last().Id, -1);
		Assert.Equal(deposits[1], deposit);
	}
}
