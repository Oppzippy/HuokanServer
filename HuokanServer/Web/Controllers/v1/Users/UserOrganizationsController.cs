using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Permissions;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Users;

[ApiController]
[Route("users/{userId:guid}/organizations")]
public class UserOrganizationsController : LoggedInControllerBase
{
	private readonly IOrganizationRepository _organizationRepository;
	private readonly IPermissionResolver _permissionResolver;

	public UserOrganizationsController(IOrganizationRepository organizationRepository, IPermissionResolver permissionResolver)
	{
		_organizationRepository = organizationRepository;
		_permissionResolver = permissionResolver;
	}

	[HttpGet]
	[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.USER)]
	public async Task<ActionResult<OrganizationCollectionModel>> GetOrganizationsContainingUser([FromRoute(Name = "userId")] Guid userId)
	{
		if (userId == User.Id)
		{
			// Me
			List<BackedOrganization> organizations = await _organizationRepository.FindOrganizationsContainingUser(userId);
			return OrganizationCollectionModel.From(organizations);
		}
		if (await _permissionResolver.DoesUserHaveGlobalPermission(User, GlobalPermission.ADMINISTRATOR))
		{
			// A user that isn't me
			List<BackedOrganization> organizations = await _organizationRepository.FindOrganizationsContainingUser(userId);
			return OrganizationCollectionModel.From(organizations);
		}
		return NotFound();
	}
}