using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class GetGuildsFromNonexistentOrganizationTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetGuildsFromNonexistentOrganization()
		{
			HttpResponseMessage response = await HttpClient.GetAsync($"{BaseUrl}/organizations/{Guid.Empty}/guilds");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
