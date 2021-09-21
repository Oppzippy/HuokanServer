using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers.v1.Organizations
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
		[Authorize(Policy = "User")]
		public async Task<GetOrganizationsResponse> GetOrganizations()
		{
			List<BackedOrganization> organizations = await _organizationRepository.FindOrganizationsContainingUser(_user.Id);
			return new GetOrganizationsResponse()
			{
				Organizations = organizations.Select(ApiOrganization.From)
			};
		}

		[HttpPost]
		[Authorize(Policy = "Administrator")]
		public async Task<ApiOrganization> CreateOrganization([FromBody] ApiOrganization apiOrganization)
		{
			BackedOrganization newOrganization = await _organizationRepository.CreateOrganization(apiOrganization.ToOrganization());
			return ApiOrganization.From(newOrganization);
		}
	}
}
