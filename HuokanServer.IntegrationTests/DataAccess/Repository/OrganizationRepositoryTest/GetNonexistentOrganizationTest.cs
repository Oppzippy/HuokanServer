using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.OrganizationRepositoryTest
{
	public class GetNonexistentOrganizationTest : OrganizationRepositoryTestBase
	{
		[Fact]
		public async Task TestGetNonexistentOrganization()
		{
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.GetOrganization(Guid.Empty));
		}
	}
}
