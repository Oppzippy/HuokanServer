using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Organizations
{
	[ApiController]
	[Route("organizations")]
	public class OrganizationsController : ControllerBase
	{
		private readonly IOrganizationRepository _organizationRepository;
		private BackedUser _user
		{
			get
			{
				return HttpContext.Features.Get<BackedUser>();
			}
		}

		public OrganizationsController(IOrganizationRepository organizationRepository)
		{
			_organizationRepository = organizationRepository;
		}

		[HttpGet]
		[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.USER)]
		public async Task<GetOrganizationsResponse> GetOrganizations()
		{
			List<BackedOrganization> organizations = await _organizationRepository.FindOrganizationsContainingUser(_user.Id);
			return new GetOrganizationsResponse()
			{
				Organizations = organizations.Select(ApiOrganization.From)
			};
		}

		[HttpPost]
		[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.ADMINISTRATOR)]
		public async Task<ApiOrganization> CreateOrganization([FromBody] ApiOrganization apiOrganization)
		{
			BackedOrganization newOrganization = await _organizationRepository.CreateOrganization(apiOrganization.ToOrganization());
			return ApiOrganization.From(newOrganization);
		}
	}
}
