using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class GetGuildTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetGuild()
		{
			GuildModel fetchedGuild = await HttpClient.GetFromJsonAsync<GuildModel>($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}");
			Assert.Equal(Guild.Id, fetchedGuild.Id);
		}
	}
}
