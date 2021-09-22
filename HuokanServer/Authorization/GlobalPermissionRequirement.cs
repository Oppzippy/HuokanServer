using HuokanServer.Models.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Authorization;

namespace HuokanServer.Authorization
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
