using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.DepositRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record DepositModel
{
	[Required]
	[BindRequired]
	[Range(0, int.MaxValue)]
	public int Endorsements { get; init; }

	[Required]
	[BindRequired]
	[MinLength(2)]
	[MaxLength(12)]
	public string CharacterName { get; init; }

	[Required]
	[BindRequired]
	[MinLength(1)]
	[MaxLength(100)]
	public string CharacterRealm { get; set; }

	[Required]
	[BindRequired]
	[Range(typeof(long), "1", "99999999999")]
	public long DepositInCopper { get; init; }

	public static DepositModel From(BackedDeposit deposit)
	{
		return new DepositModel()
		{
			CharacterName = deposit.CharacterName,
			CharacterRealm = deposit.CharacterRealm,
			DepositInCopper = deposit.DepositInCopper,
			Endorsements = deposit.Endorsements,
		};
	}
}