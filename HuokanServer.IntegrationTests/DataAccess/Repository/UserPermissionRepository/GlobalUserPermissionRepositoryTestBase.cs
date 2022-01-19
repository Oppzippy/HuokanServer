using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.IntegrationTests.TestBases;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.UserPermissionRepository;

public class GlobalUserPermissionRepositoryTestBase : DatabaseTestBase
{
	public IGlobalUserPermissionRepository Repository
	{
		get
		{
			return new GlobalUserPermissionRepository(ConnectionFactory);
		}
	}
}