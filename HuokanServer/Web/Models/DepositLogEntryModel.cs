using HuokanServer.DataAccess.Repository.DepositRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record DepositLogEntryModel
	{
		[BindRequired]
		public string CharacterName { get; init; }
		[BindRequired]
		public long DepositInCopper { get; init; }
		[BindRequired]
		public long GuildBankCopper { get; init; }

		public Deposit ToDeposit()
		{
			return new Deposit()
			{
				CharacterName = CharacterName,
				DepositInCopper = DepositInCopper,
				GuildBankCopper = GuildBankCopper,
			};
		}
	}
}
