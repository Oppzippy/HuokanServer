using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.DepositRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record DepositLogEntryModel
{
	[Required]
	[BindRequired]
	[MinLength(2)]
	[MaxLength(12)]
	public string CharacterName { get; init; }

	[Required]
	[BindRequired]
	[MinLength(1)]
	[MaxLength(100)]
	public string CharacterRealm { get; init; }

	[Required]
	[BindRequired]
	[Range(typeof(long), "1", "99999999999")] // 10m gold - 1 copper
	public long DepositInCopper { get; init; }

	[Required]
	[BindRequired]
	[Range(typeof(long), "0", "99999999999")]
	public long GuildBankCopper { get; init; }

	public Deposit ToDeposit()
	{
		return new Deposit()
		{
			CharacterName = CharacterName,
			CharacterRealm = CharacterRealm,
			DepositInCopper = DepositInCopper,
			GuildBankCopper = GuildBankCopper,
		};
	}
}
