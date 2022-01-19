using System;

namespace HuokanServer.DataAccess.Repository.OrganizationRepository;

public record BackedOrganization : Organization
{
	public Guid Id { get; init; }
	public DateTimeOffset CreatedAt { get; init; }
}
