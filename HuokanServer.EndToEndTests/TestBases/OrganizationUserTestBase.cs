using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;

namespace HuokanServer.EndToEndTests.TestBases
{
	public class OrganizationUserTestBase : HttpTestBase
	{
		protected readonly BackedUser User;
		protected readonly BackedUser AdminUser;
		protected readonly BackedOrganization Organization;
		protected readonly BackedGuild Guild;
		protected readonly HttpClient HttpClient;
		protected readonly HttpClient AdminHttpClient;

		public OrganizationUserTestBase() : base()
		{
			var organizationRepository = new OrganizationRepository(ConnectionFactory);
			var createUser = CreateUser();
			var createAdminUser = CreateAdminUser();
			var createOrganization = organizationRepository.CreateOrganization(new Organization()
			{
				Name = "Organization One",
				Slug = "organization-one",
				DiscordGuildId = 1,
			});

			Task.WaitAll(createUser, createAdminUser, createOrganization);

			AdminUser = createAdminUser.Result;
			User = createUser.Result;
			Organization = createOrganization.Result;

			var guildRepository = new GuildRepository(ConnectionFactory);
			var globalUserPermissionRepository = new GlobalUserPermissionRepository(ConnectionFactory);
			var userRepository = new UserRepository(ConnectionFactory);
			var createGuild = guildRepository.CreateGuild(new Guild()
			{
				Name = "GuildName",
				Realm = "Realm",
				OrganizationId = Organization.Id,
			});
			var createHttpClient = GetHttpClient(User);
			var createAdminHttpClient = GetHttpClient(AdminUser);

			Task.WaitAll(
				createGuild,
				createHttpClient,
				createAdminHttpClient,
				globalUserPermissionRepository.SetIsAdministrator(AdminUser.Id, true),
				userRepository.AddUserToOrganization(User.Id, Organization.Id),
				userRepository.AddUserToOrganization(AdminUser.Id, Organization.Id)
			);

			Guild = createGuild.Result;
			HttpClient = createHttpClient.Result;
			AdminHttpClient = createAdminHttpClient.Result;
		}
	}
}
