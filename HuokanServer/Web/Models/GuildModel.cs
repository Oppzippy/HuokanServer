using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.GuildRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record GuildModel
	{
		public Guid? Id { get; init; }

		[BindRequired]
		[Required]
		public string Name { get; init; }

		[BindRequired]
		[Required]
		public string Realm { get; init; }

		public static GuildModel From(BackedGuild backedGuild)
		{
			return new GuildModel()
			{
				Id = backedGuild.Id,
				Name = backedGuild.Name,
				Realm = backedGuild.Realm,
			};
		}
	}
}
