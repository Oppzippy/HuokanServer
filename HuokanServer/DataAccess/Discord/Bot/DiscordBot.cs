using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using HuokanServer.DataAccess.Cache.DiscordGuildMember;

namespace HuokanServer.DataAccess.Discord.Bot
{
	public class DiscordBot : IDiscordBot
	{
		private readonly DiscordClient _client;
		private readonly DiscordGuildMemberCache _cache;

		public DiscordBot(DiscordClient client, DiscordGuildMemberCache cache)
		{
			_client = client;
			_cache = cache;
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
			DiscordGuildMember discordGuildMember = await _cache.GetGuildMember(guildId, userId);
			if (discordGuildMember != null)
			{
				return discordGuildMember;
			}

			DiscordGuild guild = await _client.GetGuildAsync(guildId);
			DiscordMember member = await guild.GetMemberAsync(userId);

			discordGuildMember = DiscordGuildMember.FromDSharpPlus(member);
			await _cache.SetGuildMember(guildId, userId, discordGuildMember);

			return discordGuildMember;
		}
	}
}
