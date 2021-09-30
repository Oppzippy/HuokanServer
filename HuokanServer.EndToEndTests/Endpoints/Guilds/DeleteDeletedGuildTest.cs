using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class DeleteDeletedGuildTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestDeleteDeletedGuild()
		{
			await HttpClient.DeleteAsync($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}");
			HttpResponseMessage response = await HttpClient.DeleteAsync($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}");
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
