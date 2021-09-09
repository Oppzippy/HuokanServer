using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.DepositRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers.v1.Organizations.Guilds.Deposits
{
	[Route("organizations/{organizationId}/guilds/{guildId}/deposits")]
	public class DepositsController : ControllerBase
	{
		private readonly IDepositRepository _depositRepository;

		public DepositsController(IDepositRepository depositRepository)
		{
			_depositRepository = depositRepository;
		}

		[HttpGet]
		[Authorize(Policy = "OrganizationModerator")]
		public async Task<List<BackedDeposit>> GetDeposits(
			[FromRoute(Name = "organizationId")] Guid organizationId,
			[FromRoute(Name = "guildId")] Guid guildId
		)
		{
			return await _depositRepository.GetDeposits(organizationId, guildId);
		}
	}
}
