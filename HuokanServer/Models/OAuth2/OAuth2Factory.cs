using System.Net.Http;

namespace HuokanServer.Models.OAuth2
{
	public class OAuth2Factory : IOAuth2Factory
	{
		private readonly HttpClient _httpClient;

		public OAuth2Factory(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public IOAuth2 CreateDiscord(string clientId, string clientSecret)
		{
			return new DiscordOAuth2(_httpClient, clientId, clientSecret);
		}
	}
}
