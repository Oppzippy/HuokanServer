namespace HuokanServer.Models.Discord
{
	public class DiscordUserFactory : IDiscordUserFactory
	{
		public IDiscordUser Create(string oauthToken)
		{
			return new DiscordUser(oauthToken);
		}
	}
}
