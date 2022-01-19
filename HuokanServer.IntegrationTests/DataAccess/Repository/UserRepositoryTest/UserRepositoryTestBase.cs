using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.IntegrationTests.TestBases;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserRepositoryTest;

public class UserRepositoryTestBase : DatabaseTestBase
{
	public IUserRepository Repository
	{
		get
		{
			return new UserRepository(ConnectionFactory);
		}
	}
}
