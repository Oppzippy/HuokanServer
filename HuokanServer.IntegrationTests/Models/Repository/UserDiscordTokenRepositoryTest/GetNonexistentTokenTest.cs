using System.Threading.Tasks;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.UserDiscordTokenRepositoryTest
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
