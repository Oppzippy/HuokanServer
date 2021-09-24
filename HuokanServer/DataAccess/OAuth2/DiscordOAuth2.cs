using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.WebUtilities;

namespace HuokanServer.DataAccess.OAuth2
{
	public class DiscordOAuth2 : IOAuth2
	{
		private const string AUTHORIZATION_URI = "https://discord.com/api/oauth2/authorize";
		private const string TOKEN_URI = "https://discord.com/api/oauth2/token";
		private const string TOKEN_REVOCATION_URI = "https://discord.com/api/oauth2/token/revoke";

		public string ClientId { get; }
		public string ClientSecret { get; }

		private HttpClient _httpClient;

		public DiscordOAuth2(HttpClient httpClient, string clientId, string clientSecret)
		{
			_httpClient = httpClient;
			ClientId = clientId;
			ClientSecret = clientSecret;
		}

		public string GetAuthorizationUri(string redirectUri)
		{
			var queryParams = new Dictionary<string, string>(){
				{ "client_id", ClientId },
				{ "redirect_uri", redirectUri },
				{ "response_type", "code" },
				{ "scope", "identify" },
			};

			return QueryHelpers.AddQueryString(AUTHORIZATION_URI, queryParams);
		}

		public async Task<TokenResponse> GetToken(string code, string redirectUri)
		{
			TokenResponse response = await _httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest()
			{
				Address = TOKEN_URI,
				ClientId = ClientId,
				ClientSecret = ClientSecret,
				Code = code,
				RedirectUri = redirectUri,
			});
			if (response.IsError)
			{
				throw new OAuth2Exception(response.Error, response.Exception);
			}
			return response;
		}

		public async Task<TokenResponse> RefreshToken(string refreshToken)
		{
			TokenResponse response = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest()
			{
				Address = TOKEN_URI,
				ClientId = ClientId,
				ClientSecret = ClientSecret,
				RefreshToken = refreshToken,
			});
			if (response.IsError)
			{
				throw new OAuth2Exception(response.Error);
			}
			return response;
		}

		public async Task<TokenRevocationResponse> RevokeToken(string token)
		{
			TokenRevocationResponse response = await _httpClient.RevokeTokenAsync(new TokenRevocationRequest()
			{
				Address = TOKEN_REVOCATION_URI,
				ClientId = ClientId,
				ClientSecret = ClientSecret,
				Token = token,
			});
			if (response.IsError)
			{
				throw new OAuth2Exception(response.Error);
			}
			return response;
		}

		public record DiscordOAuth2TokenResponse
		{
			public string AccessToken { get; set; }
			public string TokenType { get; set; }
			public int ExpiresIn { get; set; }
			public string RefreshToken { get; set; }
			public string Scope { get; set; }
		}
	}
}
