using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record AuthorizationModel
	{
		[BindRequired]
		[Required]
		public string ApiKey { get; init; }
	}
}
