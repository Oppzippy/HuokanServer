using System;
using HuokanServer.Models.Repository.DepositRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository
{
	public class OrganizationRepositoryTest : IClassFixture<DatabaseFixture>
	{
		private readonly DatabaseFixture _dbFixture;

		public OrganizationRepositoryTest(DatabaseFixture dbFixture)
		{
			this._dbFixture = dbFixture;
		}

		[Fact]
		public async void TestCreateOrganization()
		{
			var repo = new OrganizationRepository(_dbFixture.DbConnection);
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
