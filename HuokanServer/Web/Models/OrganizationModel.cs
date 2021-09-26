using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record OrganizationModel
	{
		public Guid? Id { get; init; }

		[BindRequired]
		[Required]
		public string Name { get; init; }

		[BindRequired]
		[Required]
		public string Slug { get; init; }

		[BindRequired]
		[Required]
		public ulong DiscordGuildId { get; init; }

		public DateTime? CreatedAt { get; init; }

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
