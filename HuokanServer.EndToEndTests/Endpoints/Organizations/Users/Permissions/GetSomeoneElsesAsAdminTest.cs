using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Organizations.Users.Permissions
{
	public class GetSomeoneElsesAsAdmin : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetSomeoneElsesPermissionsAsAdmin()
		{
			HttpResponseMessage response = await AdminHttpClient.GetAsync(
				$"{BaseUrl}/organizations/{Organization.Id}/users/{User.Id}/permissions"
			);
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
