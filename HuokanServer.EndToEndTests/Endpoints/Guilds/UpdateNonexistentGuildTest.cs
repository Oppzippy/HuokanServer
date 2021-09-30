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
	public class UpdateNonexistentGuildTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestUpdateNonexistentGuild()
		{
			HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(
				$"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guid.Empty}",
				new GuildModel()
				{
					Name = "Test",
					Realm = "Test",
				}
			);
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
