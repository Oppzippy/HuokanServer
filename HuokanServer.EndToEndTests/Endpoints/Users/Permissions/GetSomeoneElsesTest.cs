using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users.Permissions
{
	public class GetSomeoneElsesTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetSomeoneElsesPermissions()
		{
			HttpResponseMessage response = await AdminHttpClient.GetAsync($"{BaseUrl}/users/{User.Id}/permissions");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
