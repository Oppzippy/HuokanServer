﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.IntegrationTests.TestBases;

namespace HuokanServer.IntegrationTests.DataAccess.Repository.GuildRepositoryTest;

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
