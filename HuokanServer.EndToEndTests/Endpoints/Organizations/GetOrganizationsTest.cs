using System;
using System.Collections.Generic;
using System.Linq;
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
	public class GetOrganizationsTest : HttpTestBase
	{
		[Fact]
		public async Task TestGetOrganizations()
		{
			(HttpClient client, BackedUser user) = await GetHttpClient();
			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			BackedOrganization organization = await organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization One",
				Slug = "organization-one",
			});
			await organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 2,
				Name = "Organization Two",
				Slug = "organization-two",
			});

			var userRepository = new UserRepository(ConnectionFactory);
			await userRepository.AddUserToOrganization(user.Id, organization.Id);

			OrganizationCollectionModel organizations = await client.GetFromJsonAsync<OrganizationCollectionModel>($"{BaseUrl}/organizations");
			Assert.Single(organizations.Organizations);
			Assert.Equal(organization.Id, organizations.Organizations.First().Id);
		}
	}
}
