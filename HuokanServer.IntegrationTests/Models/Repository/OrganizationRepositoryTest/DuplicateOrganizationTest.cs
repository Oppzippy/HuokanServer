using System;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestPresets;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	[Collection("Database")]
	public class DuplicateOrganizationTest : DatabaseTestPreset
	{
		[Theory]
		[ClassData(typeof(OrganizationDuplicateFieldData))]
		public async Task TestDuplicateFields(Organization organization1, Organization organization2)
		{
			var repo = new OrganizationRepository(DbConnection);
			await repo.CreateOrganization(organization1);
			// TODO create exception for duplicate
			await Assert.ThrowsAnyAsync<Exception>(() => repo.CreateOrganization(organization2));
		}
	}
}
