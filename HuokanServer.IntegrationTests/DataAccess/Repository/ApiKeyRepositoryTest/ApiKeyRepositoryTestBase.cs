using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.IntegrationTests.TestBases;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.ApiKeyRepositoryTest;

public class ApiKeyRepositoryTestBase : DatabaseTestBase
{
	public IApiKeyRepository Repository
	{
		get
		{
			return new ApiKeyRepository(ConnectionFactory);
		}
	}

	protected async Task<BackedUser> CreateUser()
	{
		var userRepository = new UserRepository(ConnectionFactory);
		return await userRepository.CreateUser(new User()
		{
			DiscordUserId = 1,
		});
	}
}