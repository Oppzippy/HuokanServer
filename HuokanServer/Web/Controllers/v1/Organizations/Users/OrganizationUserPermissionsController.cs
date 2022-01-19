using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Permissions;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Organizations.Users;

[ApiController]
[Route("organizations/{organizationId:guid}/users/{userId:guid}/permissions")]
public class OrganizationUserPermissionsController : LoggedInControllerBase
{
	private readonly IPermissionResolver _permissionResolver;
	private readonly IOrganizationRepository _organizationRepository;

	public OrganizationUserPermissionsController(IPermissionResolver permissionResolver, IOrganizationRepository organizationRepository)
	{
		_permissionResolver = permissionResolver;
		_organizationRepository = organizationRepository;
	}

	[HttpGet]
	[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.USER)]
	public async Task<ActionResult<OrganizationPermissionCollectionModel>> GetOrganizationUserPermissions(
		[FromRoute(Name = "organizationId")] Guid organizationId,
		[FromRoute(Name = "userId")] Guid userId
	)
	{
		if (userId != User.Id)
		{
			// Not implemented
			return NotFound();
		}

		var permissions = new HashSet<OrganizationPermissionModel>();
		foreach (var permissionModel in Enum.GetValues<OrganizationPermissionModel>())
		{
			var permission = Enum.Parse<OrganizationPermission>(permissionModel.ToString());
			if (await _permissionResolver.DoesUserHaveOrganizationPermission(User, organizationId, permission))
			{
				permissions.Add(permissionModel);
			}
		}

		return new OrganizationPermissionCollectionModel()
		{
			Permissions = permissions,
		};
	}
}