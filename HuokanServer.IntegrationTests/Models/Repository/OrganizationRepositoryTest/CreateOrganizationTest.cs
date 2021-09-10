using System;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestPresets;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	[Collection("Database")]
	public class OrganizationTest : DatabaseTestPreset
	{
		[Fact]
		public async Task TestCreate()
		{
			var repo = new OrganizationRepository(DbConnection);
			var organization = await repo.CreateOrganization(new Organization()
			{
				DiscordGuildId = 123,
				Name = "Test Organization",
				Slug = "test-organization",
			});
			Assert.NotEqual(Guid.Empty, organization.Id);
			Assert.NotEqual(default(DateTime), organization.CreatedAt);
			Assert.Equal<ulong>(123, organization.DiscordGuildId);
			Assert.Equal("Test Organization", organization.Name);
			Assert.Equal("test-organization", organization.Slug);
		}
	}
}
