using System;

namespace HuokanServer.DataAccess.Repository.DepositRepository
{
	public record CreateDepositsArgs
	{
		public int? ParentNodeId { get; init; }
		public Guid GuildId { get; init; }
		public string CharacterName { get; init; }
		public long DepositInCopper { get; init; }
		public long GuildBankCopper { get; init; }
		public DateTime CreatedAt { get; init; }
	}
}