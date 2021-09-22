using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using HuokanServer.Models.Discord;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.Models.Repository.UserPermissionRepository
{
	public class DiscordUserPermissionRepository : IUserPermissionRepository
	{
		private readonly IDiscordUser _discordUser;
		// TODO replace with some sort of TTL cache (MemoryCache?)
		private readonly Dictionary<ulong, DiscordMember> _currentUserMemberCache = new Dictionary<ulong, DiscordMember>();

		public DiscordUserPermissionRepository(IDiscordUser discordUser)
		{
			_discordUser = discordUser;
		}

		public bool IsGlobalAdministrator()
		{
			// TODO check database for this
			return _discordUser.Id == 191587255557554177u; // Oppzippy#2963
		}

		public async Task<bool> IsOrganizationAdministrator(BackedOrganization organization)
		{
			DiscordMember member = await GetCurrentUserGuildMember(organization.DiscordGuildId);
			return IsGlobalAdministrator() || member.IsOwner || member.Permissions.HasPermission(DSharpPlus.Permissions.Administrator);
		}

		public async Task<bool> IsOrganizationModerator(BackedOrganization organization)
		{
			DiscordMember member = await GetCurrentUserGuildMember(organization.DiscordGuildId);
			return IsGlobalAdministrator() || member.IsOwner || member.Permissions.HasPermission(DSharpPlus.Permissions.ManageGuild);
		}

		public async Task<bool> IsOrganizationMember(BackedOrganization organization)
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
