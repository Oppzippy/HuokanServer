using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Organizations
{
	public class CreateOrganizationTest : HttpTestBase
	{
		[Fact]
		public async Task TestCreateOrganization()
		{
			HttpClient client = await GetAdminHttpClient();
			HttpResponseMessage[] responses = await Task.WhenAll(
				client.PostAsJsonAsync($"{BaseUrl}/organizations", new OrganizationModel()
				{
					Name = "Organization 1",
					Slug = "organization-1",
					DiscordGuildId = 1,
				}),
				client.PostAsJsonAsync($"{BaseUrl}/organizations", new OrganizationModel()
				{
					Name = "Organization 2",
					Slug = "organization-2",
					DiscordGuildId = 2,
				})
			);

			Assert.Equal(HttpStatusCode.OK, responses[0].StatusCode);
			Assert.Equal(HttpStatusCode.OK, responses[1].StatusCode);

			var organizations = await client.GetFromJsonAsync<OrganizationCollectionModel>($"{BaseUrl}/organizations");
			Assert.Equal(2, organizations.Organizations.Count());
		}
	}
}
