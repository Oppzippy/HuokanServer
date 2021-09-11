using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class DuplicateOrganizationTest : DatabaseTestBase
	{
		[Theory]
		[ClassData(typeof(OrganizationDuplicateFieldData))]
		public async Task TestDuplicateFields(Organization organization1, Organization organization2)
		{
			var repo = new OrganizationRepository(DbConnection);
			await repo.CreateOrganization(organization1);
			// TODO create exception for duplicate
			await Assert.ThrowsAnyAsync<DuplicateItemException>(() => repo.CreateOrganization(organization2));
		}
	}
}
