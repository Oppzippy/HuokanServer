using System.Collections.Generic;

namespace HuokanServer.Web.Models
{
	public record OrganizationCollectionModel
	{
		public IEnumerable<OrganizationModel> Organizations { get; init; }
	}
}
