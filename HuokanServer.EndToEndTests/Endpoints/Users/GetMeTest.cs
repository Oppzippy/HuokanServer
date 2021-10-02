using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users
{
	public class GetMeTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetMe()
		{
			UserModel me = await HttpClient.GetFromJsonAsync<UserModel>($"{BaseUrl}/users/me");
			Assert.Equal(User.Id, me.Id);
			Assert.Equal(User.DiscordUserId, me.DiscordUserId);
		}
	}
}
