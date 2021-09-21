namespace HuokanServer
{
	public record ApplicationSettings
	{
		public string DiscordClientId { get; init; }
		public string DiscordClientSecret { get; init; }
		public string BaseUrl { get; init; }
		public string DiscordRedirectUrl { get; set; }
		public string DbConnectionString { get; init; }
	}
}
