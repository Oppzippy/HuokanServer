using HuokanServer.DataAccess.Discord;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository
{
	public class OrganizationUserPermissionRepositoryFactory : IOrganizationUserPermissionRepositoryFactory
	{
		public IOrganizationUserPermissionRepository CreateDiscord(IDiscordUser user)
		{
			return new OrganizationUserPermissionRepository(user);
		}
	}
}
