using System;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Organizations.Guilds
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
		[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.MEMBER)]
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
