using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users
{
	public class GetSomeoneElseWithoutPermissionTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetSomeoneElseWithoutPermission()
		{
			HttpResponseMessage response = await HttpClient.GetAsync($"{BaseUrl}/users/{AdminUser.Id}");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
