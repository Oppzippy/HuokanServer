using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest
{
	public class UpdateGuildTest : GuildRepositoryTestBase
	{
		[Fact]
		public async Task TestUpdate()
		{
			BackedOrganization organization = await CreateOrganization();
			BackedGuild unmodifiedGuild = await Repository.CreateGuild(new Guild()
			{
				Name = "Guild one",
				Realm = "Realm one",
				OrganizationId = organization.Id,
			});
			BackedGuild modifiedGuild = unmodifiedGuild with
			{
				Name = "Guild two",
				Realm = "Realm two",
				CreatedAt = DateTime.MinValue,
			};
			BackedGuild updatedGuild = await Repository.UpdateGuild(modifiedGuild);

			Assert.Equal(modifiedGuild.Name, updatedGuild.Name);
			Assert.Equal(modifiedGuild.Realm, updatedGuild.Realm);
			Assert.Equal(unmodifiedGuild.CreatedAt, updatedGuild.CreatedAt);
			Assert.Equal(unmodifiedGuild.OrganizationId, updatedGuild.OrganizationId);
			Assert.Equal(unmodifiedGuild.Id, updatedGuild.Id);
		}
	}
}
