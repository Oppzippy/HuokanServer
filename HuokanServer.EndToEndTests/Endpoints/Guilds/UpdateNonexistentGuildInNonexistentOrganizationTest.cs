using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Helpers;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class UpdateNonexistentGuildInNonexistentOrganizationTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestUpdateNonexistentGuildInNonexistentOrganization()
		{
			HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(
				$"{BaseUrl}/organizations/{Guid.Empty}/guilds/{Guid.Empty}",
				new GuildModel()
				{
					Name = "Name",
					Realm = "Realm",
				}
			);
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
