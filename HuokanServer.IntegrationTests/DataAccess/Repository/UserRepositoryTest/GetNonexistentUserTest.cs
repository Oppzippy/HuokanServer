using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest;

public class GetNonexistentUserTest : UserRepositoryTestBase
{
	[Fact]
	public async Task TestGetNonexistentUser()
	{
		await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.GetUser(Guid.Empty));
	}
}
