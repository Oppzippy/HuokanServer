using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record DepositLog
	{
		[BindRequired]
		public List<DepositLogEntry> Log { get; init; }
		[BindRequired]
		public DateTime CapturedAt { get; init; }
	}
}
