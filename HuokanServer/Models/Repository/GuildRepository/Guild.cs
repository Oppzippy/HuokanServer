namespace HuokanServer.Models.Repository.GuildRepository
{
	public record Guild
	{
		public int OrganizationId { get; init; }
		public string Name { get; init; }
		public string Realm { get; init; }
	}
}
