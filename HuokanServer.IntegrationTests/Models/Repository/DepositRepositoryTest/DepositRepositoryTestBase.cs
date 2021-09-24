using System;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserRepository;

namespace HuokanServer.IntegrationTests.Models.Repository.DepositRepositoryTest
{
	public class DepositRepositoryTestBase : DatabaseTestBase
	{
		public IDepositRepository Repository
		{
			get
			{
				return new DepositRepository(ConnectionFactory, new DepositImportExecutorFactory(ConnectionFactory));
			}
		}

		public async Task<BackedGuild> CreateGuild()
		{
			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			var guildRepository = new GuildRepository(ConnectionFactory);

			BackedOrganization organization = await organizationRepository.CreateOrganization(new Organization()
			{
				DiscordGuildId = 1,
				Name = "Organization",
				Slug = "organization",
			});
			return await guildRepository.CreateGuild(new Guild()
			{
				OrganizationId = organization.Id,
				Name = "Name",
				Realm = "Realm",
			});
		}

		public async Task<BackedUser> CreateUser(Guid organizationId, ulong discordUserId = 1)
		{
			var userRepository = new UserRepository(ConnectionFactory);
			BackedUser user = await userRepository.CreateUser(new User()
			{
				DiscordUserId = discordUserId,
			});
			await userRepository.AddUserToOrganization(user.Id, organizationId);
			return user;
		}
	}
}
