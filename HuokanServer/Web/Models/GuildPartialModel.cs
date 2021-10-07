using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.GuildRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record GuildPartialModel
	{
		[BindRequired]
		[Required]
		public string Name { get; init; }

		[BindRequired]
		[Required]
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
