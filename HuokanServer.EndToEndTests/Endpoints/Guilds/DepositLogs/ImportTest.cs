using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds.DepositLogs
{
	public class ImportTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestImportDepositLog()
		{
			HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}/depositLogs", new DepositLogModel()
			{
				Log = new List<DepositLogEntryModel>()
				{
					new DepositLogEntryModel()
					{
						CharacterName = "Test-Test",
						DepositInCopper = 1,
						GuildBankCopper = 1,
					},
				},
				CapturedAt = DateTimeOffset.UtcNow,
			});

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			var depositRepository = new DepositRepository(ConnectionFactory, new DepositImportExecutorFactory(ConnectionFactory));
			List<BackedDeposit> deposits = await depositRepository.GetDeposits(Organization.Id, Guild.Id);
			Assert.Single(deposits);
		}
	}
}
