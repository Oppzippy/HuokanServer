using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
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

			OrganizationModel[] organizations = await Task.WhenAll(
				responses[0].Content.ReadFromJsonAsync<OrganizationModel>(),
				responses[1].Content.ReadFromJsonAsync<OrganizationModel>()
			);

			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			await organizationRepository.GetOrganization((Guid)organizations[0].Id);
			await organizationRepository.GetOrganization((Guid)organizations[1].Id);
		}
	}
}
