using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Organizations
{
	public class GetOrganizationsAsNormalUserTest : HttpTestBase
	{
		[Fact]
		public async Task TestGetOrganizationsAsNormalUser()
		{
			BackedUser user = await CreateUser();
			HttpClient client = await GetHttpClient(user);
			HttpResponseMessage response = await client.GetAsync($"{BaseUrl}/organizations");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
