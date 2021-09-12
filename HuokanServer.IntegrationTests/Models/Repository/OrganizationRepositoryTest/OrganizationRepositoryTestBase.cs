using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
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
}
