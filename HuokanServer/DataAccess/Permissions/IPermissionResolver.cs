using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;

namespace HuokanServer.DataAccess.Permissions
{
	public interface IPermissionResolver
	{
		Task<bool> DoesUserHaveOrganizationPermission(BackedUser user, BackedOrganization organization, OrganizationPermission permission);
		Task<bool> DoesUserHaveGlobalPermission(BackedUser user, GlobalPermission permission);
	}
}
