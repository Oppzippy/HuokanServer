using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.DepositRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record DepositModel
{
	[Required]
	[BindRequired]
	public Guid Id { get; init; }
	
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
	public string CharacterRealm { get; init; }

	[Required]
	[BindRequired]
	[Range(typeof(long), "1", "99999999999")]
	public long DepositInCopper { get; init; }
	
	[Required]
	[BindRequired]
	public DateTimeOffset ApproximateDepositTimestamp { get; init; }
	
	public static DepositModel From(BackedDeposit deposit)
	{
		return new DepositModel()
		{
			Id = deposit.Id,
			CharacterName = deposit.CharacterName,
			CharacterRealm = deposit.CharacterRealm,
			DepositInCopper = deposit.DepositInCopper,
			Endorsements = deposit.Endorsements,
			ApproximateDepositTimestamp = deposit.ApproximateDepositTimestamp,
		};
	}
}
