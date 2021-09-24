using System.Threading.Tasks;
using HuokanServer.DataAccess.Permissions;
using HuokanServer.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HuokanServer.Web.Authorization
{
	public class GlobalPermissionRequirementHandler : AuthorizationHandler<GlobalPermissionRequirement>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IPermissionResolver _permissionResolver;

		public GlobalPermissionRequirementHandler(IHttpContextAccessor httpContextAccessor, IPermissionResolver permissionResolver)
		{
			_httpContextAccessor = httpContextAccessor;
			_permissionResolver = permissionResolver;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GlobalPermissionRequirement requirement)
		{
			BackedUser user = _httpContextAccessor.HttpContext.Features.Get<BackedUser>();
			if (user == null)
			{
				return;
			}
			if (await _permissionResolver.DoesUserHaveGlobalPermission(user, requirement.RequiredPermission))
			{
				context.Succeed(requirement);
			}
		}
	}
}
