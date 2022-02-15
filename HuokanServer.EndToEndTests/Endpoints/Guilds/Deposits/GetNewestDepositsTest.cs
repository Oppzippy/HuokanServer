using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds.Deposits;

public class GetNewestDepositsTest : OrganizationUserTestBase
{
	[Fact]
	public async Task TestOlderPage()
	{
		var depositRepository =
			new DepositRepository(ConnectionFactory, new DepositImportExecutorFactory(ConnectionFactory));

		await depositRepository.Import(
			Guild.OrganizationId, Guild.Id, User.Id, new List<Deposit>()
			{
				new() {
					CharacterName = "Name1",
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
				}
			});
		
		var deposits = await HttpClient.GetFromJsonAsync<DepositCollectionModel>(
			$"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}/deposits?direction=older&limit=1");

		Assert.Single(deposits.Deposits);
		Assert.Equal("Name2", deposits.Deposits.First().CharacterName);
	}
}
