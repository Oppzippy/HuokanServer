using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record OrganizationPartialModel
{
	[Required]
	[BindRequired]
	[MinLength(2)]
	[MaxLength(50)]
	public string Name { get; init; }

	[Required]
	[BindRequired]
	[MinLength(2)]
	[MaxLength(50)]
	[RegularExpression("^[A-Za-z0-9\\-]+$")]
	public string Slug { get; init; }

	[Required]
	[BindRequired]
	public ulong DiscordGuildId { get; init; }

	public static OrganizationPartialModel From(Organization organization)
	{
		return new OrganizationPartialModel()
		{
			Name = organization.Name,
			Slug = organization.Slug,
			DiscordGuildId = organization.DiscordGuildId,
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
