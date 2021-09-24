using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Authorization;

namespace HuokanServer.Web.Authorization
{
	public class GlobalPermissionRequirement : IAuthorizationRequirement
	{
		public GlobalPermission RequiredPermission { get; }

		public GlobalPermissionRequirement(GlobalPermission requiredPermission)
		{
			RequiredPermission = requiredPermission;
		}
	}
}
