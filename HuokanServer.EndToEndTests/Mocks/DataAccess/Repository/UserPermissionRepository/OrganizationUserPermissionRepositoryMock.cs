using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;

namespace HuokanServer.EndToEndTests.Mocks.DataAccess.Repository.UserPermissionRepository
{
	public class OrganizationUserPermissionRepositoryMock : IOrganizationUserPermissionRepository
	{
		public Task<bool> IsAdministrator(BackedOrganization organization, Guid huokanUserId, ulong userId)
		{
			return Task.FromResult(true);
		}

		public Task<bool> IsMember(BackedOrganization organization, Guid huokanUserId, ulong userId)
		{
			return Task.FromResult(true);
		}

		public Task<bool> IsModerator(BackedOrganization organization, Guid huokanUserId, ulong userId)
		{
			return Task.FromResult(true);
		}
	}
}
