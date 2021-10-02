using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Permissions;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Users
{
	[ApiController]
	[Route("users/{userId}/organizations")]
	public class UsersOrganizationsController : LoggedInControllerBase
	{
		private readonly OrganizationRepository _organizationRepository;
		private readonly PermissionResolver _permissionResolver;

		public UsersOrganizationsController(OrganizationRepository organizationRepository, PermissionResolver permissionResolver)
		{
			_organizationRepository = organizationRepository;
			_permissionResolver = permissionResolver;
		}

		[HttpGet]
		public async Task<ActionResult<OrganizationCollectionModel>> GetOrganizations([FromRoute] Guid userId)
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
}
