using System;
using System.Net;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Permissions;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace HuokanServer.Web.Filters
{
	public class OrganizationPermissionAuthorizationFilter : IAsyncAuthorizationFilter
	{
		public OrganizationPermission RequiredPermission { get; set; }
		private readonly IPermissionResolver _permissionResolver;
		private readonly IOrganizationRepository _organizationRepository;

		public OrganizationPermissionAuthorizationFilter(IPermissionResolver permissionResolver, IOrganizationRepository organizationRepository)
		{
			_permissionResolver = permissionResolver;
			_organizationRepository = organizationRepository;
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
			try
			{
				Guid organizationId = GetOrganizationIdFromRoute(context.HttpContext);

				if (await _permissionResolver.DoesUserHaveOrganizationPermission(user.Id, organizationId, RequiredPermission))
				{
					// Logged in and authorized
					return;
				}
			}
			catch (FormatException) { }
			catch (ItemNotFoundException) { }

			context.Result = new StatusCodeResult((int)HttpStatusCode.NotFound);
		}

		private Guid GetOrganizationIdFromRoute(HttpContext httpContext)
		{
			RouteValueDictionary routeValues = httpContext.Request.RouteValues;
			if (routeValues.TryGetValue("organizationId", out object organizationIdObject) && organizationIdObject is string organizationIdString)
			{
				return Guid.Parse(organizationIdString);
			}
			throw new FormatException();
		}
	}
}
