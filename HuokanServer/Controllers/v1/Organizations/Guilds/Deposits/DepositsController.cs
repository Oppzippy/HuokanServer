using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.DepositRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers.v1.Organizations.Guilds.Deposits
{
	public class DepositsController : ControllerBase
	{
		private readonly DepositRepository _depositRepository;

		public DepositsController(DepositRepository depositRepository)
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
