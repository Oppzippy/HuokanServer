using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Repository.OrganizationRepository
{
	public interface IOrganizationRepository
	{
		Task<BackedOrganization> CreateOrganization(Organization organization);
		Task<BackedOrganization> FindOrganization(string slug);
		Task<BackedOrganization> FindOrganization(ulong discordGuildId);
		Task<List<BackedOrganization>> FindOrganizationsContainingUser(Guid userId);
		Task<List<BackedOrganization>> GetAllOrganizations();
		Task<BackedOrganization> GetOrganization(Guid organizationId);
	}
}
