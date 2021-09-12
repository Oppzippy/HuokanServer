namespace HuokanServer
{
	public record ApplicationSettings
	{
		public string DiscordClientId { get; init; }
		public string DiscordClientSecret { get; init; }
		public string DiscordRedirectUri { get; init; }
		public string DbConnectionString { get; init; }
	}
}
