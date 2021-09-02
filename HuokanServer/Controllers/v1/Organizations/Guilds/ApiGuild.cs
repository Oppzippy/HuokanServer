using System;
using HuokanServer.Models.Repository.GuildRepository;

namespace HuokanServer.Controllers.v1.Organizations.Guilds
{
	public record ApiGuild
	{
		public Guid Id { get; init; }
		public string Name { get; init; }
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
