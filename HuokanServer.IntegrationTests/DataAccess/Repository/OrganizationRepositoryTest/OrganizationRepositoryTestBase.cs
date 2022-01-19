using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.IntegrationTests.TestBases;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.OrganizationRepositoryTest;

public class OrganizationRepositoryTestBase : DatabaseTestBase
{
	public IOrganizationRepository Repository
	{
		get
		{
			return new OrganizationRepository(ConnectionFactory);
		}
	}
}
