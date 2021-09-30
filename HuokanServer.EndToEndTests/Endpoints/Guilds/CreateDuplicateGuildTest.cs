using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class CreateDuplicateGuildTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestCreateGuild()
		{
			var guild = new GuildModel()
			{
				Name = "Test Guild",
				Realm = "TestRealm",
			};
			await HttpClient.PostAsJsonAsync(
				$"{BaseUrl}/organizations/{Organization.Id}/guilds",
				guild
			);
			HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
				$"{BaseUrl}/organizations/{Organization.Id}/guilds",
				guild
			);
			Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
		}
	}
}
