using HuokanServer.Models.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Authorization;

namespace HuokanServer.Authorization
{
	public class OrganizationPermissionRequirement : IAuthorizationRequirement
	{
		public OrganizationPermission RequiredPermission { get; }

		public OrganizationPermissionRequirement(OrganizationPermission requiredPermission)
		{
			RequiredPermission = requiredPermission;
		}
	}
}
