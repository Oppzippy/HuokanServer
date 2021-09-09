namespace HuokanServer.Controllers.v1.Authorization.Discord
{
	public record DiscordAuthorizeResponse
	{
		public string ApiKey { get; init; }
	}
}
