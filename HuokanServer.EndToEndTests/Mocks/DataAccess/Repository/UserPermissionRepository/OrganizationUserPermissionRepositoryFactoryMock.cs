using HuokanServer.DataAccess.Discord;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;

namespace HuokanServer.EndToEndTests.Mocks.DataAccess.Repository.UserPermissionRepository
{
	public class OrganizationUserPermissionRepositoryFactoryMock : IOrganizationUserPermissionRepositoryFactory
	{
		public IOrganizationUserPermissionRepository CreateDiscord(IDiscordUser user)
		{
			return new OrganizationUserPermissionRepositoryMock();
		}
	}
}
