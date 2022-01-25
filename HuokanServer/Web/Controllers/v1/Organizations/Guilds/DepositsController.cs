using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

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
		[FromQuery(Name = "after")] Guid? afterDepositId = null,
		[FromQuery(Name = "limit")] [Range(1, 100)]
		int limit = 50
	)
	{
		List<BackedDeposit> deposits;
		if (afterDepositId != null)
		{
			deposits = await _depositRepository.GetDepositsAfter(organizationId, guildId, afterDepositId, limit);
		}
		else
		{
			deposits = await _depositRepository.GetDeposits(organizationId, guildId, limit);
		}

		return DepositCollectionModel.From(deposits);
	}

	[HttpGet]
	[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.MODERATOR)]
	[Route("{depositId:guid}")]
	public async Task<BackedDeposit> GetDeposit(
		[FromRoute(Name = "organizationId")] Guid organizationId,
		[FromRoute(Name = "guildId")] Guid guildId,
		[FromRoute(Name = "depositId")] Guid depositId,
		[FromQuery(Name = "offset")] [Range(int.MinValue, 0)]
		int offset
	)
	{
		BackedDeposit deposit = await _depositRepository.GetDeposit(organizationId, guildId, depositId, offset);
		return deposit;
	}
}
