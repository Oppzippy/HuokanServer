using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.DataAccess.Repository.DepositRepository;

public interface IDepositRepository
{
	Task<List<BackedDeposit>> GetDeposits(Guid organizationId, Guid guildId);
	Task Import(Guid organizationId, Guid guildId, Guid userId, List<Deposit> deposits);
}