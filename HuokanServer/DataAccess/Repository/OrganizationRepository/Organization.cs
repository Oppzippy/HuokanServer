namespace HuokanServer.DataAccess.Repository.OrganizationRepository;

public record Organization
{
	public string Name { get; init; }
	public string Slug { get; init; }
	public ulong DiscordGuildId { get; init; }
}