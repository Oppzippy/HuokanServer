using System.Threading.Tasks;
using HuokanServer.Models.Repository.DepositRepository;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers.v1.Organizations.Guilds.DepositLogs
{
	[Route("depositLogs")]
	public class DepositLogsController : ControllerBase
	{
		private readonly DepositRepository _depositRepository;

		public DepositLogsController(DepositRepository depositRepository)
		{
			_depositRepository = depositRepository;
		}

		public async Task ImportDepositLog([FromBody] DepositLog depositLog)
		{

		}
	}
}
