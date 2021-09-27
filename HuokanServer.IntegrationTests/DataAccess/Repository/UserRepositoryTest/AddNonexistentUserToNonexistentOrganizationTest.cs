using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest
{
	public class AddNonexistentUserToNonexistentOrganizationTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestAddNonexistentUserToNonexistentOrganization()
		{
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.AddUserToOrganization(Guid.Empty, Guid.Empty));
		}
	}
}
