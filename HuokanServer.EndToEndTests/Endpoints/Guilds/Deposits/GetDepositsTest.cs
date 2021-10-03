using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds.Deposits
{
	public class GetDepositsTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetDeposits()
		{
			var depositRepository = new DepositRepository(ConnectionFactory, new DepositImportExecutorFactory(ConnectionFactory));
			await depositRepository.Import(Organization.Id, Guild.Id, User.Id, new List<Deposit>()
			{
				new Deposit()
				{
					CharacterName = "Test-Test",
					DepositInCopper = 1,
					GuildBankCopper = 1,
				}
			});

			var depositCollection = await HttpClient.GetFromJsonAsync<DepositCollectionModel>($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}/deposits");
			List<DepositModel> deposits = depositCollection.Deposits;
			Assert.Single(deposits);
			Assert.Equal("Test-Test", deposits.First().CharacterName);
			Assert.Equal(1, deposits.First().DepositInCopper);
			Assert.Equal(1, deposits.First().Endorsements);
		}
	}
}
