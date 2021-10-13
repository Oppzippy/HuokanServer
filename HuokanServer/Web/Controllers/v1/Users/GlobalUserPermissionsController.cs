using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Permissions;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Users
{
	[ApiController]
	[Route("users/{userId:guid}/permissions")]
	public class GlobalUserPermissionsController : LoggedInControllerBase
	{
		private readonly IPermissionResolver _permissionResolver;

		public GlobalUserPermissionsController(IPermissionResolver permissionResolver)
		{
			_permissionResolver = permissionResolver;
		}

		[HttpGet]
		public async Task<ActionResult<GlobalPermissionCollectionModel>> GetUserPermissions([FromRoute(Name = "userId")] Guid userId)
		{
			if (userId != User.Id)
			{
				// TODO Not implemented
				return NotFound();
			}
			var permissions = new HashSet<GlobalPermissionModel>();
			foreach (var permissionModel in Enum.GetValues<GlobalPermissionModel>())
			{
				var permission = Enum.Parse<GlobalPermission>(permissionModel.ToString());
				if (await _permissionResolver.DoesUserHaveGlobalPermission(userId, permission))
				{
					permissions.Add(permissionModel);
				}
			}
			return new GlobalPermissionCollectionModel()
			{
				Permissions = permissions,
			};
		}
	}
}
