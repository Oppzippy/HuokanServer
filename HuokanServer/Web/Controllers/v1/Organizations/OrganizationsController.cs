using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Organizations
{
	[ApiController]
	[Route("organizations")]
	public class OrganizationsController : LoggedInControllerBase
	{
		private readonly IOrganizationRepository _organizationRepository;

		public OrganizationsController(IOrganizationRepository organizationRepository)
		{
			_organizationRepository = organizationRepository;
		}

		[HttpGet]
		[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.ADMINISTRATOR)]
		public async Task<OrganizationCollectionModel> GetOrganizations()
		{
			List<BackedOrganization> organizations = await _organizationRepository.GetAllOrganizations();
			return new OrganizationCollectionModel()
			{
				Organizations = organizations.Select(OrganizationModel.From).ToList()
			};
		}

		[HttpPost]
		[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.ADMINISTRATOR)]
		public async Task<OrganizationModel> CreateOrganization([FromBody] OrganizationPartialModel apiOrganization)
		{
			BackedOrganization newOrganization = await _organizationRepository.CreateOrganization(apiOrganization.ToOrganization());
			return OrganizationModel.From(newOrganization);
		}
	}
}
