using HuokanServer.Models.Discord;

namespace HuokanServer.Models.Repository.UserPermissionRepository
{
	public interface IUserPermissionRepositoryFactory
	{
		IUserPermissionRepository CreateDiscord(IDiscordUser user);
	}
}
