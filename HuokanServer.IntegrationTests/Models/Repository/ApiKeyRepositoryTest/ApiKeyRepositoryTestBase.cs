using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.UserRepository;

namespace HuokanServer.IntegrationTests.Models.Repository.ApiKeyRepositoryTest
{
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
}
