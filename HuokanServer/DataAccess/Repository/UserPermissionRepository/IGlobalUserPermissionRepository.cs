using System;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository;

public interface IGlobalUserPermissionRepository
{
	Task<bool> IsAdministrator(Guid userId);
	Task SetIsAdministrator(Guid userId, bool isAdministrator);
}