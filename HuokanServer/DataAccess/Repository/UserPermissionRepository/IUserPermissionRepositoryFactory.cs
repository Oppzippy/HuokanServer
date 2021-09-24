using HuokanServer.DataAccess.Discord;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository
{
	public interface IUserPermissionRepositoryFactory
	{
		IUserPermissionRepository CreateDiscord(IDiscordUser user);
	}
}
