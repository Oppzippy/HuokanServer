using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.DataAccess.Repository.UserRepository;

namespace HuokanServer.IntegrationTests.Models.Repository.UserRepositoryTest
{
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
}
