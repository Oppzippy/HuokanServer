using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Users.Permissions
{
	public class GetMineAsAdminTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetMyPermissionsAsAdmin()
		{
			var permissionCollection = await AdminHttpClient.GetFromJsonAsync<GlobalPermissionCollectionModel>(
				$"{BaseUrl}/users/{AdminUser.Id}/permissions",
				SerializerOptions
			);
			Assert.Equal(new HashSet<GlobalPermissionModel>()
			{
				GlobalPermissionModel.ADMINISTRATOR,
			}, permissionCollection.Permissions);
		}
	}
}
