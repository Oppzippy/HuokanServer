namespace HuokanServer.Models.Repository.ApiKeyRepository
{
	public record BackedApiKey : ApiKey
	{
		public int Id { get; init; }
		public string HashedKey { get; init; }
	}
}
