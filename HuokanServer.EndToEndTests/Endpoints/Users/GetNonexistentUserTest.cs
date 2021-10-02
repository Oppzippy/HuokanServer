using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users
{
	public class GetNonexistentUserTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetNonexistentUser()
		{
			HttpResponseMessage response = await AdminHttpClient.GetAsync($"{BaseUrl}/users/{Guid.Empty}");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
