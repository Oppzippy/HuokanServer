using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.GuildRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record GuildPartialModel
	{
		[Required]
		[BindRequired]
		[MinLength(2)]
		[MaxLength(24)]
		public string Name { get; init; }

		[Required]
		[BindRequired]
		[MinLength(1)]
		[MaxLength(50)]
		public string Realm { get; init; }

		public static GuildPartialModel From(Guild guild)
		{
			return new GuildModel()
			{
				Name = guild.Name,
				Realm = guild.Realm,
			};
		}
	}
}
