using System;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.Controllers.v1.Organizations
{
	public record ApiOrganization
	{
		public Guid Id { get; init; }
		public string Name { get; init; }
		public string Slug { get; init; }
		public ulong DiscordGuildId { get; init; }
		public DateTime CreatedAt { get; init; }

		public static ApiOrganization From(BackedOrganization backedOrganization)
		{
			return new ApiOrganization()
			{
				Id = backedOrganization.Id,
				Name = backedOrganization.Name,
				Slug = backedOrganization.Slug,
				DiscordGuildId = backedOrganization.DiscordGuildId,
				CreatedAt = backedOrganization.CreatedAt,
			};
		}

		public Organization ToOrganization()
		{
			return new Organization()
			{
				Name = Name,
				Slug = Slug,
				DiscordGuildId = DiscordGuildId,
			};
		}
	}
}
