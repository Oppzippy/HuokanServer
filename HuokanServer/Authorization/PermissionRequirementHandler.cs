using System;
using System.Threading.Tasks;
using HuokanServer.Models.Discord;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserPermissionRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace HuokanServer.Authorization
{
	public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IUserPermissionRepositoryFactory _userPermissionRepositoryFactory;
		private readonly IDiscordUserFactory _discordUserFactory;

		public PermissionRequirementHandler(IHttpContextAccessor httpContextAccessor, IOrganizationRepository organizationRepository, IUserPermissionRepositoryFactory userPermissionRepositoryFactory, IDiscordUserFactory discordUserFactory)
		{
			_httpContextAccessor = httpContextAccessor;
			_organizationRepository = organizationRepository;
			_userPermissionRepositoryFactory = userPermissionRepositoryFactory;
			_discordUserFactory = discordUserFactory;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			BackedUser user = _httpContextAccessor.HttpContext.Features.Get<BackedUser>();
			if (user == null)
			{
				context.Fail();
				return;
			}
			if (requirement.RequiredPermission == Permission.USER)
			{
				context.Succeed(requirement);
				return;
			}
			RouteValueDictionary routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
			if (routeValues.TryGetValue("organizationId", out object organizationIdObject) && organizationIdObject is string organizationIdString)
			{
				Guid organizationId = Guid.Parse(organizationIdString);
				BackedOrganization org = await _organizationRepository.GetOrganization(organizationId);


				var userPermissionRepository = _userPermissionRepositoryFactory.CreateDiscord(
					await _discordUserFactory.Create(user.Id)
				);

				switch (requirement.RequiredPermission)
				{
					case Permission.ORGANIZATION_ADMINISTRATOR:
						if (await userPermissionRepository.IsOrganizationAdministrator(org))
						{
							context.Succeed(requirement);
						}
						break;
					case Permission.ORGANIZATION_MODERATOR:
						if (await userPermissionRepository.IsOrganizationModerator(org))
						{
							context.Succeed(requirement);
						}
						break;
					case Permission.ORGANIZATION_MEMBER:
						if (await userPermissionRepository.IsOrganizationMember(org))
						{
							context.Succeed(requirement);
						}
						break;
				}
			}
		}
	}
}
