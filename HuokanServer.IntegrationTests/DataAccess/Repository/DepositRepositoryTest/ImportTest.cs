using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.DepositRepositoryTest;

public class ImportTest : DepositRepositoryTestBase
{
	[Fact]
	public async Task TestImport()
	{
		BackedGuild guild = await CreateGuild();
		BackedUser user = await CreateUser(guild.OrganizationId);
		await Repository.Import(guild.OrganizationId, guild.Id, user.Id, new List<Deposit>()
		{
			new Deposit(){
				CharacterName = "Advertiser",
				CharacterRealm = "Illidan",
				DepositInCopper = 1,
				GuildBankCopper = 1,
				ApproximateDepositTimestamp = DateTimeOffset.UtcNow,
			},
			new Deposit(){
				CharacterName = "Advertiser2",
				CharacterRealm = "Illidan",
				DepositInCopper = 2,
				GuildBankCopper = 3,
				ApproximateDepositTimestamp = DateTimeOffset.UtcNow,
			},
		});
	}
}
