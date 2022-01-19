using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord.User;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;

namespace HuokanServer.DataAccess.Permissions;

public class PermissionResolver : IPermissionResolver
{
	private readonly IOrganizationUserPermissionRepository _organizationUserPermissionRepository;
	private readonly IUnknownDiscordUserFactory _discordUserFactory;
	private readonly IGlobalUserPermissionRepository _globalUserPermissionRepository;
	private readonly IOrganizationRepository _organizationRepository;

	public PermissionResolver(IOrganizationUserPermissionRepository organizationUserPermissionRepository, IUnknownDiscordUserFactory discordUserFactory, IGlobalUserPermissionRepository globalUserPermissionRepository, IOrganizationRepository organizationRepository)
	{
		_organizationUserPermissionRepository = organizationUserPermissionRepository;
		_discordUserFactory = discordUserFactory;
		_globalUserPermissionRepository = globalUserPermissionRepository;
		_organizationRepository = organizationRepository;
	}

	public async Task<bool> DoesUserHaveOrganizationPermission(BackedUser user, Guid organizationId, OrganizationPermission permission)
	{
		BackedOrganization organization = await _organizationRepository.GetOrganization(organizationId);

		switch (permission)
		{
			case OrganizationPermission.ADMINISTRATOR:
				return await _organizationUserPermissionRepository.IsAdministrator(organization, user.Id, user.DiscordUserId);
			case OrganizationPermission.MODERATOR:
				return await _organizationUserPermissionRepository.IsModerator(organization, user.Id, user.DiscordUserId);
			case OrganizationPermission.MEMBER:
				return await _organizationUserPermissionRepository.IsMember(organization, user.Id, user.DiscordUserId);
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
