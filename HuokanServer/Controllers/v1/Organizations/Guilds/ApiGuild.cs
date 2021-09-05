using System;
using HuokanServer.Models.Repository.GuildRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Controllers.v1.Organizations.Guilds
{
	public record ApiGuild
	{
		public Guid Id { get; init; }
		[BindRequired]
		public string Name { get; init; }
		[BindRequired]
		public string Realm { get; init; }

		public static ApiGuild From(BackedGuild backedGuild)
		{
			return new ApiGuild()
			{
				Id = backedGuild.Id,
				Name = backedGuild.Name,
				Realm = backedGuild.Name,
			};
		}
	}
}
