using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository
{
	public interface IOrganizationUserPermissionRepository
	{
		Task<bool> IsAdministrator(BackedOrganization organization, Guid user);
		Task<bool> IsModerator(BackedOrganization organization, Guid user);
		Task<bool> IsMember(BackedOrganization organization, Guid user);
	}
}
