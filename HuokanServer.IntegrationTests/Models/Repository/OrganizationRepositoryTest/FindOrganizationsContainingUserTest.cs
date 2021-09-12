using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class FindOrganizationsContainingUserTest : OrganizationRepositoryTestBase
	{
		[Fact]
		public async Task TestFindContainingUser()
		{
			var userRepository = new UserRepository(ConnectionFactory);
			BackedOrganization[] organizations = await Task.WhenAll(
				Repository.CreateOrganization(new Organization()
				{
					Name = "Organization one",
					Slug = "organization-one",
					DiscordGuildId = 1,
				}),
				Repository.CreateOrganization(new Organization()
				{
					Name = "Organization two",
					Slug = "organization-two",
					DiscordGuildId = 2,
				}),
				Repository.CreateOrganization(new Organization()
				{
					Name = "Organization three",
					Slug = "organization-three",
					DiscordGuildId = 3,
				})
			);
			BackedUser user = await userRepository.CreateUser(new User()
			{
				DiscordUserId = 1,
			});
			await userRepository.AddUserToOrganization(user.Id, organizations[0].Id);
			await userRepository.AddUserToOrganization(user.Id, organizations[1].Id);

			Assert.Equal(
				new HashSet<BackedOrganization>(organizations.Take(2)),
				new HashSet<BackedOrganization>(await Repository.FindOrganizationsContainingUser(user.Id))
			);
		}
	}
}
