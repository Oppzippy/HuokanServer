using System;

namespace HuokanServer.Models.Repository.GuildRepository
{
	public record Guild
	{
		public Guid OrganizationId { get; init; }
		public string Name { get; init; }
		public string Realm { get; init; }
	}
}
