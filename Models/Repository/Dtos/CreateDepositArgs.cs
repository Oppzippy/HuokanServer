using System;

namespace HuokanServer.Models.Repository.Dtos
{
	public class CreateDepositsArgs
	{
		public int? ParentNodeId { get; set; }
		public int GraphId { get; set; }
		public string CharacterName { get; set; }
		public long DepositInCopper { get; set; }
		public long GuildBankCopper { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
