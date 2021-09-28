using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record GuildCollectionModel
	{
		[BindRequired]
		[Required]
		public List<GuildModel> Guilds { get; init; }
	}
}
