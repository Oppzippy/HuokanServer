using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models
{
	public record OrganizationPartialModel
	{
		[BindRequired]
		[Required]
		public string Name { get; init; }

		[BindRequired]
		[Required]
		public string Slug { get; init; }

		[BindRequired]
		[Required]
		public string DiscordGuildId { get; init; }

		public static OrganizationPartialModel From(Organization organization)
		{
			return new OrganizationPartialModel()
			{
				Name = organization.Name,
				Slug = organization.Slug,
				DiscordGuildId = organization.DiscordGuildId.ToString(),
			};
		}

		public Organization ToOrganization()
		{
			// TODO wrap exception from ulong.parse
			return new Organization()
			{
				Name = Name,
				Slug = Slug,
				DiscordGuildId = ulong.Parse(DiscordGuildId),
			};
		}
	}
}
