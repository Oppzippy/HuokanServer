namespace HuokanServer.Models.Repository.OrganizationRepository
{
	public record Organization
	{
		public string Name { get; init; }
		public string Slug { get; init; }
	}
}
