using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.GuildRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record GuildModel : GuildPartialModel
{
	[Required]
	[BindRequired]
	public Guid Id { get; init; }

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