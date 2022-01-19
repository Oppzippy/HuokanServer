using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HuokanServer.Web.Models;

public record OrganizationCollectionModel
{
	[Required]
	[BindRequired]
	public List<OrganizationModel> Organizations { get; init; }

	public static OrganizationCollectionModel From(IEnumerable<BackedOrganization> organizations)
	{
		return new OrganizationCollectionModel()
		{
			Organizations = organizations.Select(OrganizationModel.From).ToList(),
		};
	}
}