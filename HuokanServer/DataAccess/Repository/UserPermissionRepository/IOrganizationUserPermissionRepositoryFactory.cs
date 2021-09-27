using HuokanServer.DataAccess.Discord;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository
{
	public interface IOrganizationUserPermissionRepositoryFactory
	{
		IOrganizationUserPermissionRepository CreateDiscord(IDiscordUser user);
	}
}
