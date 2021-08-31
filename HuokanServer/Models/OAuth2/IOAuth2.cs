using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace HuokanServer.Models.OAuth2
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

	public class OAuth2Exception : Exception
	{
		public OAuth2Exception() { }
		public OAuth2Exception(string message) : base(message) { }
		public OAuth2Exception(string message, Exception innerException) : base(message, innerException) { }
		protected OAuth2Exception(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
