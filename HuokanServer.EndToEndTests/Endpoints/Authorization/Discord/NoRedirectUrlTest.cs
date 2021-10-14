using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Authorization.Discord
{
	public class NoRedirectUrlTest : HttpTestBase
	{
		[Fact]
		public async Task TestRedirectWithoutRedirectUrl()
		{
			var httpClientHandler = new HttpClientHandler();
			httpClientHandler.AllowAutoRedirect = false;
			var httpClient = new HttpClient(httpClientHandler);

			HttpResponseMessage response = await httpClient.GetAsync($"{BaseUrl}/authorization/discord/redirect");
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}
