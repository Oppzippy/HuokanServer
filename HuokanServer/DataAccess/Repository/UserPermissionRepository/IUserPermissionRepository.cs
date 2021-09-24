using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository
{
	public interface IUserPermissionRepository
	{
		Task<bool> IsOrganizationAdministrator(BackedOrganization organization);
		Task<bool> IsOrganizationModerator(BackedOrganization organization);
		Task<bool> IsOrganizationMember(BackedOrganization organization);
	}
}
