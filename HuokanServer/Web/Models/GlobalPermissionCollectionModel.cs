using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record GlobalPermissionCollectionModel
{
	[Required]
	[BindRequired]
	public HashSet<GlobalPermissionModel> Permissions { get; init; }
}