using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.IntegrationTests.Models.Repository.GuildRepositoryTest
{
	public class GuildRepositoryTestBase : DatabaseTestBase
	{
		public IGuildRepository Repository
		{
			get
			{
				return new GuildRepository(ConnectionFactory);
			}
		}

		public async Task<List<BackedOrganization>> CreateOrganizations(int amount)
		{
			var repo = new OrganizationRepository(ConnectionFactory);
			var organizations = new List<BackedOrganization>();
			for (int i = 0; i < amount; i++)
			{
				organizations.Add(await repo.CreateOrganization(new Organization()
				{
					DiscordGuildId = (ulong)i + 1,
					Name = $"Test Organization {i}",
					Slug = $"test-organization-{i}",
				}));
			}
			return organizations; ;
		}

		public async Task<BackedOrganization> CreateOrganization()
		{
			return (await CreateOrganizations(1))[0];
		}
	}
}
