using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.DepositRepositoryTest;

public class FindBestPathTest : DepositRepositoryTestBase
{
	[Fact]
	public async Task TestFindBestPath()
	{
		BackedGuild guild = await CreateGuild();
		BackedUser user1 = await CreateUser(guild.OrganizationId);
		BackedUser user2 = await CreateUser(guild.OrganizationId, 2);
		BackedUser user3 = await CreateUser(guild.OrganizationId, 3);

		await Repository.Import(guild.OrganizationId, guild.Id, user1.Id, new List<Deposit>()
		{
			new Deposit()
			{
				CharacterName = "MaliciousUser",
				CharacterRealm = "Illidan",
				DepositInCopper = 999999,
				GuildBankCopper = 999999,
				ApproximateDepositTimestamp = new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero),
			},
		});
		await Repository.Import(guild.OrganizationId, guild.Id, user1.Id, new List<Deposit>()
		{
			new Deposit()
			{
				CharacterName = "Advertiser",
				CharacterRealm = "Illidan",
				DepositInCopper = 1,
				GuildBankCopper = 1,
				ApproximateDepositTimestamp = new DateTimeOffset(2022, 1, 1, 1, 0, 0, TimeSpan.Zero),
			},
			new Deposit()
			{
				CharacterName = "Advertiser2",
				CharacterRealm = "Illidan",
				DepositInCopper = 2,
				GuildBankCopper = 3,
				ApproximateDepositTimestamp = new DateTimeOffset(2022, 1, 1, 2, 0, 0, TimeSpan.Zero),
			},
		});
		await Repository.Import(guild.OrganizationId, guild.Id, user2.Id, new List<Deposit>()
		{
			new Deposit()
			{
				CharacterName = "Advertiser",
				CharacterRealm = "Illidan",
				DepositInCopper = 1,
				GuildBankCopper = 1,
				ApproximateDepositTimestamp = new DateTimeOffset(2022, 1, 1, 1, 1, 0, TimeSpan.Zero),
			},
			new Deposit()
			{
				CharacterName = "Advertiser2",
				CharacterRealm = "Illidan",
				DepositInCopper = 2,
				GuildBankCopper = 3,
				ApproximateDepositTimestamp = new DateTimeOffset(2022, 1, 1, 2, 1, 0, TimeSpan.Zero),
			},
			new Deposit()
			{
				CharacterName = "Advertiser2",
				CharacterRealm = "Illidan",
				DepositInCopper = 2,
				GuildBankCopper = 5,
				ApproximateDepositTimestamp = new DateTimeOffset(2022, 1, 1, 3, 1, 0, TimeSpan.Zero),
			},
		});

		List<BackedDeposit> deposits = await Repository.GetDeposits(guild.OrganizationId, guild.Id);
		Assert.Equal(3, deposits.Count);

		Assert.NotEqual(Guid.Empty, deposits[0].Id);
		Assert.Equal(1, deposits[0].DepositInCopper);
		Assert.Equal("Advertiser", deposits[0].CharacterName);
		Assert.Equal("Illidan", deposits[0].CharacterRealm);
		Assert.Equal(2, deposits[0].Endorsements);
		Assert.Equal(new DateTimeOffset(2022, 1, 1, 1, 0, 0, TimeSpan.Zero), deposits[0].ApproximateDepositTimestamp);

		Assert.NotEqual(Guid.Empty, deposits[1].Id);
		Assert.Equal(2, deposits[1].DepositInCopper);
		Assert.Equal("Advertiser2", deposits[1].CharacterName);
		Assert.Equal("Illidan", deposits[1].CharacterRealm);
		Assert.Equal(2, deposits[1].Endorsements);
		Assert.Equal(new DateTimeOffset(2022, 1, 1, 2, 0, 0, TimeSpan.Zero), deposits[1].ApproximateDepositTimestamp);

		Assert.NotEqual(Guid.Empty, deposits[2].Id);
		Assert.Equal(2, deposits[2].DepositInCopper);
		Assert.Equal("Advertiser2", deposits[2].CharacterName);
		Assert.Equal("Illidan", deposits[2].CharacterRealm);
		Assert.Equal(1, deposits[2].Endorsements);
		Assert.Equal(new DateTimeOffset(2022, 1, 1, 3, 1, 0, TimeSpan.Zero), deposits[2].ApproximateDepositTimestamp);
	}
}
