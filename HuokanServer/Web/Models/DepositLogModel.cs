using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record DepositLogModel
{
	[Required]
	[BindRequired]
	public List<DepositLogEntryModel> Log { get; init; }

	[Required]
	[BindRequired]
	public DateTimeOffset CapturedAt { get; init; }
}