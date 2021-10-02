using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users.Organizations
{
	public class GetSomeoneElsesTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetSomeoneElsesOrganizations()
		{
			var organizations = await AdminHttpClient.GetFromJsonAsync<OrganizationCollectionModel>($"{BaseUrl}/users/{User.Id}/organizations");
			Assert.Single(organizations.Organizations);
		}
	}
}
