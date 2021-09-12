using System;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class OrganizationTest : OrganizationRepositoryTestBase
	{
		[Fact]
		public async Task TestCreate()
		{
			var organization = await Repository.CreateOrganization(new Organization()
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
