using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users
{
	public class GetSomeoneElseTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetSomeoneElse()
		{
			UserModel user = await AdminHttpClient.GetFromJsonAsync<UserModel>($"{BaseUrl}/users/{User.Id}");
			Assert.Equal(User.Id, user.Id);
			Assert.Equal(User.DiscordUserId, user.DiscordUserId);
		}
	}
}
