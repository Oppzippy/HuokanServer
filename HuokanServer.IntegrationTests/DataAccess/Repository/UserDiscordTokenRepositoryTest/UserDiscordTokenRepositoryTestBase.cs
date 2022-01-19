using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.IntegrationTests.TestBases;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserDiscordTokenRepositoryTest;

public class UserDiscordTokenRepositoryTestBase : DatabaseTestBase
{
	public IUserDiscordTokenRepository Repository
	{
		get
		{
			return new UserDiscordTokenRepository(ConnectionFactory);
		}
	}

	public async Task<BackedUser> CreateUser()
	{
		var userRepository = new UserRepository(ConnectionFactory);
		return await userRepository.CreateUser(new User()
		{
			DiscordUserId = 1,
		});
	}
}