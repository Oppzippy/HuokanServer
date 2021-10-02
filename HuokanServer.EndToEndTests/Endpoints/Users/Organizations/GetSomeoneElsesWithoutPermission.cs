using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users.Organizations
{
	public class GetSomeoneElsesWithoutPermission : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetSomeoneElsesOrganizationsWithoutPermission()
		{
			HttpResponseMessage response = await HttpClient.GetAsync($"{BaseUrl}/users/{AdminUser.Id}/organizations");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
