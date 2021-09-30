using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class GetGuildFromNonexistentOrganizationTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetGuildFromNonexistentOrganization()
		{
			HttpResponseMessage response = await HttpClient.GetAsync($"{BaseUrl}/organizations/{Guid.Empty}/guilds/{Guid.Empty}");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
