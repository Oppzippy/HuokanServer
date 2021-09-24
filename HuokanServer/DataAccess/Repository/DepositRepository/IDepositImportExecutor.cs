using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Repository.DepositRepository
{
	public interface IDepositImportExecutor
	{
		Task Import(Guid organizationId, Guid guildId, Guid userId, List<Deposit> deposits);
	}
}
