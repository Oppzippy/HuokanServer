namespace HuokanServer.DataAccess.Repository.UserRepository;

public record User
{
	public ulong DiscordUserId { get; init; }
}