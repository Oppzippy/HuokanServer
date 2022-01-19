using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.OrganizationRepositoryTest;

public class OrganizationTest : OrganizationRepositoryTestBase
{
	[Fact]
	public async Task TestCreate()
	{
		var organization = await Repository.CreateOrganization(new Organization()
		{
			DiscordGuildId = 754272382285381653,
			Name = "Test Organization",
			Slug = "test-organization",
		});
		Assert.NotEqual(Guid.Empty, organization.Id);
		Assert.NotEqual(default(DateTimeOffset), organization.CreatedAt);
		Assert.Equal<ulong>(754272382285381653, organization.DiscordGuildId);
		Assert.Equal("Test Organization", organization.Name);
		Assert.Equal("test-organization", organization.Slug);
	}
}