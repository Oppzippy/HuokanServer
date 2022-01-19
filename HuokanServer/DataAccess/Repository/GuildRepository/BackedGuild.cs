using System;

namespace HuokanServer.DataAccess.Repository.GuildRepository;

public record BackedGuild : Guild
{
	public Guid Id { get; init; }
	public DateTimeOffset CreatedAt { get; init; }
}