using System;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record OrganizationModel
	{
		public Guid Id { get; init; }
		[BindRequired]
		public string Name { get; init; }
		[BindRequired]
		public string Slug { get; init; }
		[BindRequired]
		public ulong DiscordGuildId { get; init; }
		public DateTime CreatedAt { get; init; }

		public static OrganizationModel From(BackedOrganization backedOrganization)
		{
			return new OrganizationModel()
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
