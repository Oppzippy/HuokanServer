using System;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.DepositRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers.v1.Organizations.Guilds.DepositLogs
{
	[Route("organizations/{organizationId}/guilds/{guildId}/depositLogs")]
	public class DepositLogsController : ControllerBase
	{
		private readonly IDepositRepository _depositRepository;
		private readonly BackedUser _user;

		public DepositLogsController(IDepositRepository depositRepository)
		{
			_depositRepository = depositRepository;
			_user = HttpContext.Features.Get<BackedUser>();
		}

		[HttpPost]
		[Authorize(Policy = "OrganizationMember")]
		public async Task ImportDepositLog(
			[FromRoute(Name = "organizationId")] Guid organizationId,
			[FromRoute(Name = "guildId")] Guid guildId,
			[FromBody] DepositLog depositLog
		)
		{
			await _depositRepository.Import(
				organizationId,
				guildId,
				_user.Id,
				depositLog.Log.Select(entry => entry.ToDeposit()).ToList()
			);
		}
	}
}
