using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds.Deposits
{
	public class GetDepositsFromNonexistentGuildTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetDepositsFromNonexistentGuild()
		{
			HttpResponseMessage response = await HttpClient.GetAsync($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guid.Empty}/deposits");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
