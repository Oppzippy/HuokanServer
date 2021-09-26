using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record DepositLogModel
	{
		[BindRequired]
		[Required]
		public List<DepositLogEntryModel> Log { get; init; }

		[BindRequired]
		[Required]
		public DateTime CapturedAt { get; init; }
	}
}
