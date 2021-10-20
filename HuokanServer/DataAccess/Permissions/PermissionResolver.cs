using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord.User;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;

namespace HuokanServer.DataAccess.Permissions
{
	public class PermissionResolver : IPermissionResolver
	{
		private readonly IOrganizationUserPermissionRepository _organizationUserPermissionRepository;
		private readonly IDiscordUserFactory _discordUserFactory;
		private readonly IGlobalUserPermissionRepository _globalUserPermissionRepository;
		private readonly IOrganizationRepository _organizationRepository;

		public PermissionResolver(IOrganizationUserPermissionRepository organizationUserPermissionRepository, IDiscordUserFactory discordUserFactory, IGlobalUserPermissionRepository globalUserPermissionRepository, IOrganizationRepository organizationRepository)
		{
			_organizationUserPermissionRepository = organizationUserPermissionRepository;
			_discordUserFactory = discordUserFactory;
			_globalUserPermissionRepository = globalUserPermissionRepository;
			_organizationRepository = organizationRepository;
		}

		public async Task<bool> DoesUserHaveOrganizationPermission(Guid userId, Guid organizationId, OrganizationPermission permission)
		{
			BackedOrganization organization = await _organizationRepository.GetOrganization(organizationId);

			switch (permission)
			{
				case OrganizationPermission.ADMINISTRATOR:
					return await _organizationUserPermissionRepository.IsAdministrator(organization, userId);
				case OrganizationPermission.MODERATOR:
					return await _organizationUserPermissionRepository.IsModerator(organization, userId);
				case OrganizationPermission.MEMBER:
					return await _organizationUserPermissionRepository.IsMember(organization, userId);
				default:
					return false;
			}
		}

		public async Task<bool> DoesUserHaveGlobalPermission(Guid userId, GlobalPermission permission)
		{
			switch (permission)
			{
				case GlobalPermission.USER:
					return userId != Guid.Empty;
				case GlobalPermission.ADMINISTRATOR:
					return await _globalUserPermissionRepository.IsAdministrator(userId);
				default:
					return false;
			}
		}
	}
}
