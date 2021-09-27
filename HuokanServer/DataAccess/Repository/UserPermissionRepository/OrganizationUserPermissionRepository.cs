using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using HuokanServer.DataAccess.Discord;
using HuokanServer.DataAccess.Repository.OrganizationRepository;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository
{
	public class OrganizationUserPermissionRepository : IOrganizationUserPermissionRepository
	{
		private readonly IDiscordUser _discordUser;
		// TODO replace with some sort of TTL cache (MemoryCache?)
		private readonly Dictionary<ulong, DiscordMember> _currentUserMemberCache = new Dictionary<ulong, DiscordMember>();

		public OrganizationUserPermissionRepository(IDiscordUser discordUser)
		{
			_discordUser = discordUser;
		}

		public bool IsGlobalAdministrator()
		{
			// TODO check database for this
			return _discordUser.Id == 191587255557554177u; // Oppzippy#2963
		}

		public async Task<bool> IsAdministrator(BackedOrganization organization)
		{
			DiscordMember member = await GetCurrentUserGuildMember(organization.DiscordGuildId);
			return IsGlobalAdministrator() || member.IsOwner || member.Permissions.HasPermission(DSharpPlus.Permissions.Administrator);
		}

		public async Task<bool> IsModerator(BackedOrganization organization)
		{
			DiscordMember member = await GetCurrentUserGuildMember(organization.DiscordGuildId);
			return IsGlobalAdministrator() || member.IsOwner || member.Permissions.HasPermission(DSharpPlus.Permissions.ManageGuild);
		}

		public async Task<bool> IsMember(BackedOrganization organization)
		{
			DiscordMember member = await GetCurrentUserGuildMember(organization.DiscordGuildId);
			return IsGlobalAdministrator() || member != null;
		}

		private async Task<DiscordMember> GetCurrentUserGuildMember(ulong guildId)
		{
			return await _discordUser.GuildMember(guildId);
		}
	}
}
