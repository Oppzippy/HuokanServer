using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record OrganizationPermissionCollectionModel
{
	[Required]
	[BindRequired]
	public HashSet<OrganizationPermissionModel> Permissions { get; init; }
}
