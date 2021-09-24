using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace HuokanServer.DataAccess.Discord
{
	/// <summary>Provides a high level interface for obtaining information about a discord user.
	/// Ensure Authenticate is called before doing anything.</summary>
	public class DiscordUser : IDiscordUser
	{
		private DiscordRestClient _discord;

		public async Task Authenticate(string token)
		{
			var discord = new DiscordRestClient(new DiscordConfiguration()
			{
				Token = token,
				TokenType = TokenType.Bearer,
			});
			await discord.InitializeAsync();
			_discord = discord;
		}

		public ulong Id
		{
			get { return _discord.CurrentUser.Id; }
		}

		public List<ulong> GuildIds
		{
			get
			{
				return _discord.Guilds.Keys.ToList();
			}
		}

		public async Task<DiscordMember> GuildMember(ulong guildId)
		{
			return await _discord.GetGuildMemberAsync(guildId, Id);
		}
	}
}
