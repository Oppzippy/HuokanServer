using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Repository.DepositRepository;

public interface IDepositRepository
{
	Task<List<BackedDeposit>> GetNewerDeposits(Guid organizationId, Guid guildId, Guid? relativeNodeId,
		int limit);

	Task<List<BackedDeposit>> GetOlderDeposits(Guid organizationId, Guid guildId, Guid? relativeNodeId,
		int limit);

	Task<Guid?> GetHead(Guid organizationId, Guid guildId);

	Task Import(Guid organizationId, Guid guildId, Guid userId, List<Deposit> deposits);
}
