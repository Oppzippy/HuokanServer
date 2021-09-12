using System;
using System.Threading.Tasks;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
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
