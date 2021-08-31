namespace HuokanServer.Models.OAuth2
{
	public interface IOAuth2Factory
	{
		IOAuth2 CreateDiscord(string clientId, string clientSecret);
	}
}
