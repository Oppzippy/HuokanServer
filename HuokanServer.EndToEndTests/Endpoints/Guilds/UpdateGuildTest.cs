using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Helpers;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class UpdateGuildTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestUpdateGuild()
		{
			var guildPartial = new GuildPartialModel()
			{
				Name = "Updated Name",
				Realm = "Updated Realm",
			};
			HttpResponseMessage response = await HttpClient.PatchAsJsonAsync(
				$"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}",
				guildPartial
			);
			var guild = await response.Content.ReadFromJsonAsync<GuildModel>();
			Assert.Equal(guildPartial.Name, guild.Name);
			Assert.Equal(guildPartial.Realm, guild.Realm);
		}
	}
}
