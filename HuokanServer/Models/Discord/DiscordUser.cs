using System.Threading.Tasks;
using DSharpPlus;

namespace HuokanServer.Models.Discord
{
	public class DiscordUser : IDiscordUser
	{
		private readonly DiscordRestClient _discord;

		public DiscordUser(string oauthToken)
		{
			_discord = new DiscordRestClient(new DiscordConfiguration()
			{
				Token = oauthToken,
				TokenType = TokenType.Bearer,
			});
			Task.WaitAll(_discord.InitializeAsync());
		}

		public ulong Id
		{
			get { return _discord.CurrentUser.Id; }
		}
	}
}
