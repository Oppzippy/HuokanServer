using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Organizations.Users.Permissions
{
	public class GetMineTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetMyPermissions()
		{
			var permissionCollection = await HttpClient.GetFromJsonAsync<OrganizationPermissionCollectionModel>(
				$"{BaseUrl}/organizations/{Organization.Id}/users/{User.Id}/permissions"
			);
			Assert.Equal(new HashSet<OrganizationPermissionModel>()
			{
				OrganizationPermissionModel.ADMINISTRATOR,
				OrganizationPermissionModel.MODERATOR,
				OrganizationPermissionModel.MEMBER,
			}, permissionCollection.Permissions);
		}
	}
}
