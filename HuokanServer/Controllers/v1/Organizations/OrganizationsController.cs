using System;
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
	[Route("/organizations")]
	public class OrganizationsController : ControllerBase
	{
		private readonly OrganizationRepository _organizationRepository;
		private readonly BackedUser _user;

		public OrganizationsController(IHttpContextAccessor httpContextAccessor, OrganizationRepository organizationRepository)
		{
			_user = httpContextAccessor.HttpContext.Features.Get<BackedUser>();
			_organizationRepository = organizationRepository;
		}

		[HttpGet]
		[Authorize(Policy = "User")]
		public async Task<IEnumerable<ApiOrganization>> GetOrganizations()
		{
			List<BackedOrganization> organizations = await _organizationRepository.FindOrganizationsContainingUser(_user);
			return organizations.Select(organization => new ApiOrganization()
			{
				Id = organization.Id,
				Name = organization.Name,
				Slug = organization.Slug,
				DiscordGuildId = organization.DiscordGuildId,
				CreatedAt = organization.CreatedAt,
			});
		}

		[HttpPost]
		[Authorize(Policy = "Administrator")]
		public async Task<ApiOrganization> CreateOrganization([FromBody] Organization organizationInfo)
		{
			BackedOrganization newOrganization = await _organizationRepository.CreateOrganization(organizationInfo);
			return new ApiOrganization()
			{
				Id = newOrganization.Id,
				Name = newOrganization.Name,
				Slug = newOrganization.Slug,
				DiscordGuildId = newOrganization.DiscordGuildId,
				CreatedAt = newOrganization.CreatedAt,
			};
		}

		public record ApiOrganization
		{
			public Guid Id { get; init; }
			public string Name { get; init; }
			public string Slug { get; init; }
			public ulong DiscordGuildId { get; init; }
			public DateTime CreatedAt { get; init; }
		}
	}
}
