using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.DepositRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record DepositLogEntryModel
	{
		[BindRequired]
		[Required]
		public string CharacterName { get; init; }

		[BindRequired]
		[Required]
		public long DepositInCopper { get; init; }

		[BindRequired]
		[Required]
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
