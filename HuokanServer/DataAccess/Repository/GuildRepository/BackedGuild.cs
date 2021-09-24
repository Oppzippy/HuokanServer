using System;

namespace HuokanServer.DataAccess.Repository.GuildRepository
{
	public record BackedGuild : Guild
	{
		public Guid Id { get; init; }
		public DateTime CreatedAt { get; init; }
	}
}
