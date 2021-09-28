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
		public async Task<OrganizationCollectionModel> GetOrganizations()
		{
			List<BackedOrganization> organizations = await _organizationRepository.FindOrganizationsContainingUser(_user.Id);
			return new OrganizationCollectionModel()
			{
				Organizations = organizations.Select(OrganizationModel.From).ToList()
			};
		}

		[HttpPost]
		[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.ADMINISTRATOR)]
		public async Task<OrganizationModel> CreateOrganization([FromBody] OrganizationModel apiOrganization)
		{
			BackedOrganization newOrganization = await _organizationRepository.CreateOrganization(apiOrganization.ToOrganization());
			return OrganizationModel.From(newOrganization);
		}
	}
}
