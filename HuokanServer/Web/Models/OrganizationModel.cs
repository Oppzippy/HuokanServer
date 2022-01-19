using System;
using System.ComponentModel.DataAnnotations;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record OrganizationModel : OrganizationPartialModel
{
	[Required]
	[BindRequired]
	public Guid Id { get; init; }

	[Required]
	[BindRequired]
	public DateTimeOffset CreatedAt { get; init; }

	public static OrganizationModel From(BackedOrganization organization)
	{
		return new OrganizationModel()
		{
			Id = organization.Id,
			Name = organization.Name,
			Slug = organization.Slug,
			DiscordGuildId = organization.DiscordGuildId,
			CreatedAt = organization.CreatedAt,
		};
	}
}