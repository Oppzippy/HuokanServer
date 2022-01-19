using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;

namespace HuokanServer.DataAccess.Permissions;

public interface IPermissionResolver
{
	Task<bool> DoesUserHaveOrganizationPermission(BackedUser user, Guid organizationId, OrganizationPermission permission);
	Task<bool> DoesUserHaveGlobalPermission(BackedUser user, GlobalPermission permission);
}