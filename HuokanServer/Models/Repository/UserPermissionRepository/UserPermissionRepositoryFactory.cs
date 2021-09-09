using HuokanServer.Models.Discord;

namespace HuokanServer.Models.Repository.UserPermissionRepository
{
	public class UserPermissionRepositoryFactory : IUserPermissionRepositoryFactory
	{
		public IUserPermissionRepository CreateDiscord(IDiscordUser user)
		{
			return new DiscordUserPermissionRepository(user);
		}
	}
}
