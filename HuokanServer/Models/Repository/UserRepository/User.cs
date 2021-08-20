namespace HuokanServer.Models.Repository.UserRepository
{
	public record User
	{
		public int OrganizationId { get; init; }
		public string DiscordUserId { get; init; }
	}
}
