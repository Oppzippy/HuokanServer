using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Authorization.Discord
{
	public class RedirectTest : HttpTestBase
	{
		[Fact]
		public async Task TestRedirect()
		{
			var httpClientHandler = new HttpClientHandler();
			httpClientHandler.AllowAutoRedirect = false;
			var httpClient = new HttpClient(httpClientHandler);

			HttpResponseMessage response = await httpClient.GetAsync($"{BaseUrl}/authorization/discord/redirect?redirectUrl={BaseUrl}");
			Assert.Equal(HttpStatusCode.Found, response.StatusCode);
			Assert.StartsWith("https://discord.com/api/oauth2/authorize", response.Headers.Location.AbsoluteUri);
			Assert.Contains("client_id=", response.Headers.Location.Query);
			Assert.Contains("redirect_uri=", response.Headers.Location.Query);
			Assert.Contains("response_type=", response.Headers.Location.Query);
			Assert.Contains("scope=", response.Headers.Location.Query);
		}
	}
}
