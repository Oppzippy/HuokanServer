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
	public class GetOrganizationsTestAsAdmin : HttpTestBase
	{
		[Fact]
		public async Task TestGetOrganizationsAsAdmin()
		{
			BackedUser user = await CreateAdminUser();
			HttpClient client = await GetHttpClient(user);
			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			BackedOrganization[] createdOrganizations = await Task.WhenAll(
				organizationRepository.CreateOrganization(new Organization()
				{
					DiscordGuildId = 1,
					Name = "Organization One",
					Slug = "organization-one",
				}),
				organizationRepository.CreateOrganization(new Organization()
				{
					DiscordGuildId = 2,
					Name = "Organization Two",
					Slug = "organization-two",
				})
			);

			var userRepository = new UserRepository(ConnectionFactory);
			await userRepository.AddUserToOrganization(user.Id, createdOrganizations.First().Id);

			OrganizationCollectionModel fetchedOrganizations = await client.GetFromJsonAsync<OrganizationCollectionModel>($"{BaseUrl}/organizations");
			Assert.Equal(
				new HashSet<OrganizationModel>(createdOrganizations.Select(OrganizationModel.From)),
				new HashSet<OrganizationModel>(fetchedOrganizations.Organizations)
			);
		}
	}
}
