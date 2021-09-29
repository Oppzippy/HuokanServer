using System;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Organizations.Guilds
{
	[Route("organizations/{organizationId}/guilds/{guildId}/depositLogs")]
	public class DepositLogsController : LoggedInControllerBase
	{
		private readonly IDepositRepository _depositRepository;

		public DepositLogsController(IDepositRepository depositRepository)
		{
			_depositRepository = depositRepository;
		}

		[HttpPost]
		[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.MEMBER)]
		public async Task ImportDepositLog(
			[FromRoute(Name = "organizationId")] Guid organizationId,
			[FromRoute(Name = "guildId")] Guid guildId,
			[FromBody] DepositLogModel depositLog
		)
		{
			await _depositRepository.Import(
				organizationId,
				guildId,
				User.Id,
				depositLog.Log.Select(entry => entry.ToDeposit()).ToList()
			);
		}
	}
}
