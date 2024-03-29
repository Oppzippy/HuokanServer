using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
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
			BackedUser user = await CreateAdminUser();
			HttpClient client = await GetHttpClient(user);
			HttpResponseMessage response = await client.PostAsJsonAsync($"{BaseUrl}/organizations", new OrganizationPartialModel()
			{
				Name = "Organization 1",
				Slug = "organization-1",
				DiscordGuildId = 1,
			});

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			OrganizationModel organization = await response.Content.ReadFromJsonAsync<OrganizationModel>();

			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			await organizationRepository.GetOrganization(organization.Id);
		}
	}
}
