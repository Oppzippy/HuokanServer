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

public class PaginationTest : OrganizationUserTestBase
{
	[Fact]
	public async Task TestDepositPagination()
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
		Assert.Equal(50, page1.Deposits.Count);

		Guid lastOnPage1 = page1.Deposits.Last().Id;
		var page2 = await HttpClient.GetFromJsonAsync<DepositCollectionModel>(
			$"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}/deposits?after={lastOnPage1}");
		Assert.Equal(5, page2.Deposits.Count);

		for (int i = 0; i < 50; i++)
		{
			Assert.Equal(i, page1.Deposits[i].DepositInCopper);
		}

		for (int i = 5; i < 5; i++)
		{
			Assert.Equal(50 + i, page2.Deposits[i].DepositInCopper);
		}
	}
}
