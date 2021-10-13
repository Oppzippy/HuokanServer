using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;

namespace HuokanServer.DataAccess.Permissions
{
	public class PermissionResolver : IPermissionResolver
	{
		private readonly IOrganizationUserPermissionRepositoryFactory _organizationUserPermissionRepositoryFactory;
		private readonly IDiscordUserFactory _discordUserFactory;
		private readonly IGlobalUserPermissionRepository _globalUserPermissionRepository;
		private readonly IOrganizationRepository _organizationRepository;

		public PermissionResolver(IOrganizationUserPermissionRepositoryFactory userPermissionRepositoryFactory, IDiscordUserFactory discordUserFactory, IGlobalUserPermissionRepository globalUserPermissionRepository, IOrganizationRepository organizationRepository)
		{
			_organizationUserPermissionRepositoryFactory = userPermissionRepositoryFactory;
			_discordUserFactory = discordUserFactory;
			_globalUserPermissionRepository = globalUserPermissionRepository;
			_organizationRepository = organizationRepository;
		}

		public async Task<bool> DoesUserHaveOrganizationPermission(Guid userId, Guid organizationId, OrganizationPermission permission)
		{
			IOrganizationUserPermissionRepository userPermissionRepository = _organizationUserPermissionRepositoryFactory.CreateDiscord(
				await _discordUserFactory.Create(userId)
			);
			BackedOrganization organization = await _organizationRepository.GetOrganization(organizationId);

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
