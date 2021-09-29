using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class GetGuildsTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetGuilds()
		{
			GuildCollectionModel guilds = await HttpClient.GetFromJsonAsync<GuildCollectionModel>($"{BaseUrl}/organizations/{Organization.Id}/guilds");
			Assert.Single(guilds.Guilds);
			Assert.Equal(Guild.Id, guilds.Guilds.First().Id);
		}
	}
}
