using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;

namespace HuokanServer.DataAccess.Permissions
{
	public interface IPermissionResolver
	{
		Task<bool> DoesUserHaveOrganizationPermission(Guid userId, Guid organizationId, OrganizationPermission permission);
		Task<bool> DoesUserHaveGlobalPermission(Guid userId, GlobalPermission permission);
	}
}
