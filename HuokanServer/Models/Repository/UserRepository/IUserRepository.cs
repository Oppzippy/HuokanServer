using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HuokanServer.Models.Repository.UserRepository
{
	public interface IUserRepository
	{
		Task<BackedUser> CreateUser(User user);
		Task<BackedUser> FindOrCreateUser(User user);
		Task<BackedUser> FindUser(User user);
		Task<List<BackedUser>> FindUsersInOrganization(Guid organizationId);
		Task<BackedUser> GetUser(Guid id);
		Task<bool> IsUserInOrganization(Guid userId, Guid organizationId);
		Task SetDiscordOrganizations(Guid userId, List<ulong> guildIds);
	}
}
