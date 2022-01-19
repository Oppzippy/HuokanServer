using System.Net;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Permissions;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HuokanServer.Web.Filters;

public class GlobalPermissionAuthorizationFilter : IAsyncAuthorizationFilter
{
	public GlobalPermission RequiredPermission { get; set; }

	public bool IsReusable => false;

	private readonly IPermissionResolver _permissionResolver;

	public GlobalPermissionAuthorizationFilter(IPermissionResolver permissionResolver)
	{
		_permissionResolver = permissionResolver;
	}

	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		BackedUser user = context.HttpContext.Features.Get<BackedUser>();
		if (user == null)
		{
			// Not logged in
			context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
			return;
		}
		if (await _permissionResolver.DoesUserHaveGlobalPermission(user, RequiredPermission))
		{
			// Logged in and authorized
			return;
		}
		context.Result = new StatusCodeResult((int)HttpStatusCode.NotFound);
	}
}
