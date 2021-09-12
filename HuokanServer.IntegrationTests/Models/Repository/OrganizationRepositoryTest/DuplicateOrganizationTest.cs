using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class DuplicateOrganizationTest : OrganizationRepositoryTestBase
	{
		[Theory]
		[ClassData(typeof(OrganizationDuplicateFieldData))]
		public async Task TestDuplicateFields(Organization organization1, Organization organization2)
		{
			await Repository.CreateOrganization(organization1);
			await Assert.ThrowsAnyAsync<DuplicateItemException>(() => Repository.CreateOrganization(organization2));
		}
	}
}
