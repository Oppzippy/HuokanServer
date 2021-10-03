using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds.Deposits
{
	public class GetEmptyDepositsTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetEmptyDepositCollection()
		{
			var depositCollection = await HttpClient.GetFromJsonAsync<DepositCollectionModel>($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}/deposits");
			Assert.Empty(depositCollection.Deposits);
		}
	}
}
