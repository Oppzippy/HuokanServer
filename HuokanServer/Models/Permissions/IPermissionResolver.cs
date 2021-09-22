using System.Threading.Tasks;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserPermissionRepository;
using HuokanServer.Models.Repository.UserRepository;

namespace HuokanServer.Models.Permissions
{
	public interface IPermissionResolver
	{
		Task<bool> DoesUserHaveOrganizationPermission(BackedUser user, BackedOrganization organization, OrganizationPermission permission);
		Task<bool> DoesUserHaveGlobalPermission(BackedUser user, GlobalPermission permission);
	}
}
