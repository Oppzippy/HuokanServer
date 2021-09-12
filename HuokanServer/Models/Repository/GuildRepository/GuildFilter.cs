namespace HuokanServer.Models.Repository.GuildRepository
{
	public record GuildFilter
	{
		public string Name { get; init; }
		public string Realm { get; init; }
	}
}
