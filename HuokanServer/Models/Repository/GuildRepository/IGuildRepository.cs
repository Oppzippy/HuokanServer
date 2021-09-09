using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.Models.Repository.GuildRepository
{
	public interface IGuildRepository
	{
		Task<BackedGuild> CreateGuild(Guild guild);
		Task DeleteGuild(BackedGuild guild);
		Task<List<BackedGuild>> FindGuilds(Guild guild);
		Task<BackedGuild> GetGuild(Guid organizationId, Guid guildId);
		Task<BackedGuild> UpdateGuild(BackedGuild guild);
	}
}
