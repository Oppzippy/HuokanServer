using System.Collections.Concurrent;
using System.Threading.Tasks;
using HuokanServer.Models.Discord;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserPermissionRepository;
using HuokanServer.Models.Repository.UserRepository;

namespace HuokanServer.Models.Permissions
{
	public class PermissionResolver : IPermissionResolver
	{
		private readonly IUserPermissionRepositoryFactory _userPermissionRepositoryFactory;
		private readonly IDiscordUserFactory _discordUserFactory;

		public PermissionResolver(IUserPermissionRepositoryFactory userPermissionRepositoryFactory, IDiscordUserFactory discordUserFactory)
		{
			_userPermissionRepositoryFactory = userPermissionRepositoryFactory;
			_discordUserFactory = discordUserFactory;
		}

		public async Task<bool> DoesUserHaveOrganizationPermission(BackedUser user, BackedOrganization organization, OrganizationPermission permission)
		{
			IUserPermissionRepository userPermissionRepository = _userPermissionRepositoryFactory.CreateDiscord(
				await _discordUserFactory.Create(user.Id)
			);

			switch (permission)
			{
				case OrganizationPermission.ORGANIZATION_ADMINISTRATOR:
					return await userPermissionRepository.IsOrganizationAdministrator(organization);
				case OrganizationPermission.ORGANIZATION_MODERATOR:
					return await userPermissionRepository.IsOrganizationModerator(organization);
				case OrganizationPermission.ORGANIZATION_MEMBER:
					return await userPermissionRepository.IsOrganizationMember(organization);
				default:
					return false;
			}
		}

		public Task<bool> DoesUserHaveGlobalPermission(BackedUser user, GlobalPermission permission)
		{
			switch (permission)
			{
				case GlobalPermission.USER:
					return Task.FromResult(user != null);
				default:
					return Task.FromResult(false);
			}
		}
	}
}