using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record GlobalPermissionCollectionModel
	{
		[BindRequired]
		[Required]
		public HashSet<GlobalPermissionModel> Permissions { get; init; }
	}
}
