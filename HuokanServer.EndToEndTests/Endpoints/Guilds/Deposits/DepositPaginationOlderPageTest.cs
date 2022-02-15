using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds.Deposits;

public class DepositPaginationOlderPageTest : OrganizationUserTestBase
{
	[Fact]
	public async Task TestOlderPage()
	{
		var depositRepository =
			new DepositRepository(ConnectionFactory, new DepositImportExecutorFactory(ConnectionFactory));

		var deposits = new List<Deposit>();
		for (int i = 0; i < 55; i++)
		{
			deposits.Add(new Deposit()
			{
				CharacterName = $"Name{i}",
				CharacterRealm = $"Realm",
				DepositInCopper = i,
				GuildBankCopper = i,
				ApproximateDepositTimestamp = DateTimeOffset.UtcNow,
			});
		}

		await depositRepository.Import(Organization.Id, Guild.Id, User.Id, deposits);
		var page1 = await HttpClient.GetFromJsonAsync<DepositCollectionModel>(
			$"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}/deposits");

		Guid lastOnPage1 = page1.Deposits.Last().Id;
		var latterHalfPage = await HttpClient.GetFromJsonAsync<DepositCollectionModel>(
			$"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}/deposits?relativeTo={lastOnPage1}&direction=older&limit=25");

		var latterHalf = page1.Deposits.GetRange(25, 25);

		Assert.Equal(latterHalf.Select(deposit => deposit.Id), latterHalfPage.Deposits.Select(deposit => deposit.Id));
	}
}
