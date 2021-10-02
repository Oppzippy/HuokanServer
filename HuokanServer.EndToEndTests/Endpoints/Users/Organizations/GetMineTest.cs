using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users.Organizations
{
	public class GetMineTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetMyOrganizations()
		{
			var organizations = await HttpClient.GetFromJsonAsync<OrganizationCollectionModel>($"{BaseUrl}/users/{User.Id}/organizations");
			Assert.Single(organizations.Organizations);
			Assert.Equal(Organization.Id, organizations.Organizations.First().Id);
		}
	}
}
