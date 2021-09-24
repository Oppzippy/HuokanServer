using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.DataAccess.Repository.OrganizationRepository;

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
