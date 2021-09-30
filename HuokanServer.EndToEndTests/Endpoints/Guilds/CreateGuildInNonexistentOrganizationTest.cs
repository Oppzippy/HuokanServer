using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class CreateGuildInNonexistentOrganizationTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestCreateGuildInNonexistentOrganization()
		{
			HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
				$"{BaseUrl}/organizations/{Guid.Empty}/guilds",
				new GuildModel()
				{
					Name = "Test Guild",
					Realm = "TestRealm",
				}
			);
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
