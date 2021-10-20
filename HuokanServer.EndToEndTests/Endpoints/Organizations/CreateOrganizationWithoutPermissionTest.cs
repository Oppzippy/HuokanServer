using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Organizations
{
	public class CreateOrganizationWithoutPermissionTest : HttpTestBase
	{
		[Fact]
		public async Task TestCreateOrganization()
		{
			BackedUser user = await CreateUser();
			HttpClient client = await GetHttpClient(user);
			HttpResponseMessage response = await client.PostAsJsonAsync($"{BaseUrl}/organizations", new OrganizationPartialModel()
			{
				Name = "Organization 1",
				Slug = "organization-1",
				DiscordGuildId = "1",
			});

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => organizationRepository.FindOrganization("organization-1"));
		}
	}
}
