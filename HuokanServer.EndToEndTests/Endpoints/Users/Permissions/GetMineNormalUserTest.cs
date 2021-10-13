using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users.Permissions
{
	public class GetMineNormalUserTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetMyPermissionsAsAdmin()
		{
			var permissionCollection = await HttpClient.GetFromJsonAsync<GlobalPermissionCollectionModel>($"{BaseUrl}/users/{User.Id}/permissions");
			Assert.Empty(permissionCollection.Permissions);
		}
	}
}
