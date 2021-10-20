using System.Collections.Concurrent;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace HuokanServer.DataAccess.Discord.Bot
{
	public class DiscordBot : IDiscordBot
	{
		private readonly DiscordClient _client;
		// The DiscordBot object's lifetime is only the duration of the http request so we don't
		// have to worry about expiring anything.
		private readonly ConcurrentDictionary<string, DiscordGuildMember> _guildMemberCache = new ConcurrentDictionary<string, DiscordGuildMember>();

		public DiscordBot(DiscordClient client)
		{
			_client = client;
		}

		public async Task Connect()
		{
			await _client.ConnectAsync();
		}

		public async Task Disconnect()
		{
			await _client.DisconnectAsync();
		}

		public async Task<DiscordGuildMember> GetGuildMember(ulong guildId, ulong userId)
		{
			string key = $"{guildId}:{userId}";
			if (_guildMemberCache.TryGetValue(key, out DiscordGuildMember value))
			{
				return value;
			}

			DiscordGuild guild = await _client.GetGuildAsync(guildId);
			DiscordMember member = await guild.GetMemberAsync(userId);

			var discordGuildMember = DiscordGuildMember.FromDSharpPlus(member);
			_guildMemberCache[key] = discordGuildMember;

			return discordGuildMember;
		}
	}
}
