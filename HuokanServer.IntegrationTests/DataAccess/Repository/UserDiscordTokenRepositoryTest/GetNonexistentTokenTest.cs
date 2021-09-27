using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserDiscordTokenRepositoryTest
{
	public class GetNonexistentTokenTest : UserDiscordTokenRepositoryTestBase
	{
		[Fact]
		public async Task TestGetNonexistentToken()
		{
			BackedUser user = await CreateUser();
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => Repository.GetDiscordToken(user.Id));
		}
	}
}
