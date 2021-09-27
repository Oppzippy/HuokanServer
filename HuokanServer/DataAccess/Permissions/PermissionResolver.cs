using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;

namespace HuokanServer.DataAccess.Permissions
{
	public class PermissionResolver : IPermissionResolver
	{
		private readonly IOrganizationUserPermissionRepositoryFactory _organizationUserPermissionRepositoryFactory;
		private readonly IDiscordUserFactory _discordUserFactory;
		private readonly IGlobalUserPermissionRepository _globalUserPermissionRepository;

		public PermissionResolver(IOrganizationUserPermissionRepositoryFactory userPermissionRepositoryFactory, IDiscordUserFactory discordUserFactory, IGlobalUserPermissionRepository globalUserPermissionRepository)
		{
			_organizationUserPermissionRepositoryFactory = userPermissionRepositoryFactory;
			_discordUserFactory = discordUserFactory;
			_globalUserPermissionRepository = globalUserPermissionRepository;
		}

		public async Task<bool> DoesUserHaveOrganizationPermission(BackedUser user, BackedOrganization organization, OrganizationPermission permission)
		{
			IOrganizationUserPermissionRepository userPermissionRepository = _organizationUserPermissionRepositoryFactory.CreateDiscord(
				await _discordUserFactory.Create(user.Id)
			);

			switch (permission)
			{
				case OrganizationPermission.ADMINISTRATOR:
					return await userPermissionRepository.IsAdministrator(organization);
				case OrganizationPermission.MODERATOR:
					return await userPermissionRepository.IsModerator(organization);
				case OrganizationPermission.MEMBER:
					return await userPermissionRepository.IsMember(organization);
				default:
					return false;
			}
		}

		public async Task<bool> DoesUserHaveGlobalPermission(BackedUser user, GlobalPermission permission)
		{
			switch (permission)
			{
				case GlobalPermission.USER:
					return user != null;
				case GlobalPermission.ADMINISTRATOR:
					return await _globalUserPermissionRepository.IsAdministrator(user.Id);
				default:
					return false;
			}
		}
	}
}
