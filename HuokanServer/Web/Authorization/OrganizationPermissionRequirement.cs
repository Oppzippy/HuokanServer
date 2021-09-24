using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Authorization;

namespace HuokanServer.Web.Authorization
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
