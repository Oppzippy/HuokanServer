using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Repository.GuildRepository;

public interface IGuildRepository
{
	Task<BackedGuild> CreateGuild(Guild guild);
	Task DeleteGuild(Guid organizationId, Guid guildId);
	Task<List<BackedGuild>> FindGuilds(Guid organizationId, GuildFilter guild);
	Task<BackedGuild> GetGuild(Guid organizationId, Guid guildId);
	Task<BackedGuild> UpdateGuild(BackedGuild guild);
}
