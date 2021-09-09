using System.Threading.Tasks;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.Models.Repository.UserPermissionRepository
{
	public interface IUserPermissionRepository
	{
		Task<bool> IsOrganizationAdministrator(BackedOrganization organization);
		Task<bool> IsOrganizationModerator(BackedOrganization organization);
		Task<bool> IsOrganizationMember(BackedOrganization organization);
	}
}
