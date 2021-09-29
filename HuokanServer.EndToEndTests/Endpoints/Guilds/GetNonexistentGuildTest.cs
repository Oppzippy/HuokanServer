using System;
using System.Net;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class GetNonexistentGuildTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestGetNonexistentGuild()
		{
			var result = await HttpClient.GetAsync($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guid.Empty}");
			Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
		}
	}
}
