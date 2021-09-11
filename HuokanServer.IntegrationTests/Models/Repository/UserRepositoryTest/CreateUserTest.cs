using System;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class CreateUserTest : DatabaseTestBase
	{
		[Fact]
		public async Task TestCreateUser()
		{
			var repo = new UserRepository(DbConnection);
			BackedUser user = await repo.CreateUser(new User()
			{
				DiscordUserId = 1,
			});
			Assert.NotEqual(default(DateTime), user.CreatedAt);
			Assert.NotEqual(Guid.Empty, user.Id);
			Assert.Equal<ulong>(1, user.DiscordUserId);
		}
	}
}
