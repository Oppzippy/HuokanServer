using HuokanServer.Models.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Authorization;

namespace HuokanServer.Authorization
{
	public class PermissionRequirement : IAuthorizationRequirement
	{
		public Permission RequiredPermission { get; }

		public PermissionRequirement(Permission requiredPermission)
		{
			RequiredPermission = requiredPermission;
		}
	}
}
