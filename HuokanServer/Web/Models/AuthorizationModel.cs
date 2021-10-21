using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record AuthorizationModel
	{
		[Required]
		[BindRequired]
		public string ApiKey { get; init; }
	}
}
