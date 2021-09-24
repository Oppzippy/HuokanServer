using HuokanServer.DataAccess.Discord;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository
{
	public class UserPermissionRepositoryFactory : IUserPermissionRepositoryFactory
	{
		public IUserPermissionRepository CreateDiscord(IDiscordUser user)
		{
			return new DiscordUserPermissionRepository(user);
		}
	}
}
