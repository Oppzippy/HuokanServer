using System.Collections.Generic;

namespace HuokanServer.Web.Models
{
	public record GetOrganizationsResponse
	{
		public IEnumerable<ApiOrganization> Organizations { get; init; }
	}
}
