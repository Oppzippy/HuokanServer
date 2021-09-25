using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record DepositLogModel
	{
		[BindRequired]
		public List<DepositLogEntryModel> Log { get; init; }
		[BindRequired]
		public DateTime CapturedAt { get; init; }
	}
}
