using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository
{
	public interface IOrganizationUserPermissionRepository
	{
		Task<bool> IsAdministrator(BackedOrganization organization);
		Task<bool> IsModerator(BackedOrganization organization);
		Task<bool> IsMember(BackedOrganization organization);
	}
}
