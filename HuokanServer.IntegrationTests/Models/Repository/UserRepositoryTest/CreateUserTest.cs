using System;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
	public class CreateUserTest : UserRepositoryTestBase
	{
		[Fact]
		public async Task TestCreate()
		{
			BackedUser user = await Repository.CreateUser(new User()
			{
				DiscordUserId = 1,
			});
			Assert.NotEqual(default(DateTime), user.CreatedAt);
			Assert.NotEqual(Guid.Empty, user.Id);
			Assert.Equal<ulong>(1, user.DiscordUserId);
		}
	}
}
