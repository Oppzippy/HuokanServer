using System;

namespace HuokanServer.Models.Repository.OrganizationRepository
{
	public record BackedOrganization : Organization
	{
		public Guid Id { get; init; }
		public DateTime CreatedAt { get; init; }
	}
}
