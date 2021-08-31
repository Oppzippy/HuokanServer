namespace HuokanServer.Models.Discord
{
	public interface IDiscordUserFactory
	{
		IDiscordUser Create(string oauthToken);
	}
}
