using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.IntegrationTests.TestBases;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.OrganizationRepositoryTest;

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
