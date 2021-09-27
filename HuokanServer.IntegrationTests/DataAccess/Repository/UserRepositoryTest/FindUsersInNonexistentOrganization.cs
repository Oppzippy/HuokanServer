using System;
using System.Threading.Tasks;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest
{
	public class FindUsersInNonexistentOrganization : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestFindUsersInNonexistentOrganization()
		{
			Assert.Empty(await Repository.FindUsersInOrganization(Guid.Empty));
		}
	}
}
