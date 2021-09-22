using System;
using System.Threading.Tasks;
using HuokanServer.Models.Discord;
using HuokanServer.Models.Permissions;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserPermissionRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace HuokanServer.Authorization
{
	public class OrganizationPermissionRequirementHandler : AuthorizationHandler<OrganizationPermissionRequirement>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IPermissionResolver _permissionResolver;

		public OrganizationPermissionRequirementHandler(IHttpContextAccessor httpContextAccessor, IOrganizationRepository organizationRepository, IPermissionResolver permissionResolver)
		{
			_httpContextAccessor = httpContextAccessor;
			_organizationRepository = organizationRepository;
			_permissionResolver = permissionResolver;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OrganizationPermissionRequirement requirement)
		{
			BackedUser user = _httpContextAccessor.HttpContext.Features.Get<BackedUser>();
			if (user == null)
			{
				return;
			}
			try
			{
				Guid organizationId = GetOrganizationIdFromRoute();
				BackedOrganization organization = await _organizationRepository.GetOrganization(organizationId);

				if (await _permissionResolver.DoesUserHaveOrganizationPermission(user, organization, requirement.RequiredPermission))
				{
					context.Succeed(requirement);
				}
			}
			catch (FormatException) { }
			catch (ItemNotFoundException) { }
		}

		private Guid GetOrganizationIdFromRoute()
		{
			RouteValueDictionary routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
			if (routeValues.TryGetValue("organizationId", out object organizationIdObject) && organizationIdObject is string organizationIdString)
			{
				return Guid.Parse(organizationIdString);
			}
			throw new FormatException();
		}
	}
}