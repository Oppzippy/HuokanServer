using System.Collections.Generic;

namespace HuokanServer.Models.Discord
{
	public interface IDiscordUser
	{
		ulong Id { get; }
		List<ulong> GuildIds { get; }
	}
}
