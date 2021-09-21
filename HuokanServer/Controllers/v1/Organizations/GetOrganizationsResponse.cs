using System.Collections.Generic;

namespace HuokanServer.Controllers.v1.Organizations
{
	public record GetOrganizationsResponse
	{
		public IEnumerable<ApiOrganization> Organizations { get; init; }
	}
}
