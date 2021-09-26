using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record OrganizationCollectionModel
	{
		[BindRequired]
		[Required]
		public IEnumerable<OrganizationModel> Organizations { get; init; }
	}
}
