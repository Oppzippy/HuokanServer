using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HuokanServer.DataAccess.Repository.DepositRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record DepositCollectionModel
	{
		[BindRequired]
		[Required]
		public List<DepositModel> Deposits { get; init; }

		public static DepositCollectionModel From(IEnumerable<BackedDeposit> deposits)
		{
			return new DepositCollectionModel()
			{
				Deposits = deposits.Select(DepositModel.From).ToList(),
			};
		}
	}
}
