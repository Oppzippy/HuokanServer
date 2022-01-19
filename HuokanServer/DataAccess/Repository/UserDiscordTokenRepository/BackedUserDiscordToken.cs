using System;

namespace HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;

public record BackedUserDiscordToken : UserDiscordToken
{
	public DateTimeOffset CreatedAt { get; init; }
}
