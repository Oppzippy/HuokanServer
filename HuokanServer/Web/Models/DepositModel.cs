using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.DepositRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record DepositModel
	{
		[BindRequired]
		[Required]
		public int Endorsements { get; init; }

		[BindRequired]
		[Required]
		public string CharacterName { get; init; }

		[BindRequired]
		[Required]
		public long DepositInCopper { get; init; }

		public static DepositModel From(BackedDeposit deposit)
		{
			return new DepositModel()
			{
				CharacterName = deposit.CharacterName,
				DepositInCopper = deposit.DepositInCopper,
				Endorsements = deposit.Endorsements,
			};
		}
	}
}
