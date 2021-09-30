using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class DeleteGuildInNonexistentOrganizationTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestDeleteGuildInNonexistentOrganization()
		{
			HttpResponseMessage response = await HttpClient.DeleteAsync($"{BaseUrl}/organizations/{Guid.Empty}/guilds/{Guid.Empty}");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
