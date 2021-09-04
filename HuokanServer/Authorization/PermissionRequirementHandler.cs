using System;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserPermissionRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Npgsql.Replication.PgOutput.Messages;

namespace HuokanServer.Authorization
{
	public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly OrganizationRepository _organizationRepository;
		private readonly IUserPermissionRepository _userPermissionRepository;

		public PermissionRequirementHandler(IHttpContextAccessor httpContextAccessor, OrganizationRepository organizationRepository, IUserPermissionRepository userPermissionRepository)
		{
			_httpContextAccessor = httpContextAccessor;
			_organizationRepository = organizationRepository;
			_userPermissionRepository = userPermissionRepository;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			BackedUser user = _httpContextAccessor.HttpContext.Features.Get<BackedUser>();
			RouteValueDictionary routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
			if (routeValues.TryGetValue("organizationId", out object organizationIdObject) && organizationIdObject is string organizationIdString)
			{
				Guid organizationId = Guid.Parse(organizationIdString);
				BackedOrganization org = await _organizationRepository.GetOrganization(organizationId);

				switch (requirement.RequiredPermission)
				{
					case Permission.ORGANIZATION_ADMINISTRATOR:
						if (await _userPermissionRepository.IsOrganizationAdministrator(org))
						{
							context.Succeed(requirement);
						}
						break;
					case Permission.ORGANIZATION_MODERATOR:
						if (await _userPermissionRepository.IsOrganizationModerator(org))
						{
							context.Succeed(requirement);
						}
						break;
					case Permission.ORGANIZATION_MEMBER:
						if (await _userPermissionRepository.IsOrganizationMember(org))
						{
							context.Succeed(requirement);
						}
						break;
				}
			}
			else
			{
				// TODO add permission for logged in user
			}
		}
	}
}
