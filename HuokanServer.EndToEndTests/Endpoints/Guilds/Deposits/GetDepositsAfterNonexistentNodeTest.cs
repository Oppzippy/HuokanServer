using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds.Deposits;

public class GetDepositsAfterNonexistentNodeTest : OrganizationUserTestBase
{
	[Fact]
	public async Task TestGetDepositsAfterNonexistentNode()
	{
		var depositCollection = await HttpClient.GetFromJsonAsync<DepositCollectionModel>(
			$"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}/deposits?after={Guid.Empty}");
		Assert.Empty(depositCollection.Deposits);
	}
}
