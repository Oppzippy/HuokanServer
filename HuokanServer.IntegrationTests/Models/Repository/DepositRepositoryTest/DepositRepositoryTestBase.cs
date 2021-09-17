using System;
using System.Threading.Tasks;
using HuokanServer.IntegrationTests.TestBases;
using HuokanServer.Models.Repository.DepositRepository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserRepository;

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

		public async Task<BackedUser> CreateUser(Guid organizationId)
		{
			var userRepository = new UserRepository(ConnectionFactory);
			BackedUser user = await userRepository.CreateUser(new User()
			{
				DiscordUserId = 1,
			});
			await userRepository.AddUserToOrganization(user.Id, organizationId);
			return user;
		}
	}
}
