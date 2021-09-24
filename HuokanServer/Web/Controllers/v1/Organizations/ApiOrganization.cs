using System;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Controllers.v1.Organizations
{
	public record ApiOrganization
	{
		public Guid Id { get; init; }
		[BindRequired]
		public string Name { get; init; }
		[BindRequired]
		public string Slug { get; init; }
		[BindRequired]
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
