using System.Threading.Tasks;
using IdentityModel.Client;

namespace HuokanServer.DataAccess.OAuth2
{
	public interface IOAuth2
	{
		string ClientId { get; }
		string ClientSecret { get; }
		string GetAuthorizationUri(string redirectUri);
		Task<TokenResponse> GetToken(string code, string redirectUri);
		Task<TokenResponse> RefreshToken(string refreshToken);
		Task<TokenRevocationResponse> RevokeToken(string token);
	}
}
