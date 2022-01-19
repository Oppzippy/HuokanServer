using System;
using System.Collections.Generic;
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
		[FromRoute(Name = "guildId")] Guid guildId
	)
	{
		List<BackedDeposit> deposits = await _depositRepository.GetDeposits(organizationId, guildId);
		return DepositCollectionModel.From(deposits);
	}
}
