using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Controllers.v1.Organizations.Guilds;

[Route("organizations/{organizationId:guid}/guilds/{guildId:guid}/deposits")]
public class DepositsController : LoggedInControllerBase
{
	private readonly IDepositRepository _depositRepository;

	public DepositsController(IDepositRepository depositRepository)
	{
		_depositRepository = depositRepository;
	}

	[HttpGet]
	[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.MODERATOR)]
	public async Task<DepositCollectionModel> GetDeposits(
		[FromRoute(Name = "organizationId")] Guid organizationId,
		[FromRoute(Name = "guildId")] Guid guildId,
		[Required, BindRequired, FromQuery(Name = "direction")]
		Direction direction,
		[FromQuery(Name = "relativeTo")] Guid? relativeTo = null,
		[FromQuery(Name = "limit")] [Range(1, 100)]
		int limit = 50
	)
	{
		return DepositCollectionModel.From(direction switch
		{
			Direction.NEWER => await _depositRepository.GetNewerDeposits(organizationId, guildId, relativeTo, limit),
			Direction.OLDER => await _depositRepository.GetOlderDeposits(organizationId, guildId, relativeTo, limit),
			_ => new List<BackedDeposit>(),
		});
	}
}
