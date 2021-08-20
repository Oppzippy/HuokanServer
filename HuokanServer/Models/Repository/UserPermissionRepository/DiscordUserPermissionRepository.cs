using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserPermissionRepository;

namespace HuokanServer.Models.Repository.PermissionRepository
{
	public class DiscordUserPermissionRepository : IDisposable, IUserPermissionRepository
	{
		private readonly DiscordRestClient _discord;
		// TODO replace with some sort of TTL cache (MemoryCache?)
		private readonly Dictionary<ulong, DiscordMember> _currentUserMemberCache = new Dictionary<ulong, DiscordMember>();

		public DiscordUserPermissionRepository(string oauthToken)
		{
			_discord = new DiscordRestClient(new DiscordConfiguration()
			{
				Token = oauthToken,
				TokenType = TokenType.Bearer,
			});
			_discord.InitializeAsync();
		}

		public void Dispose()
		{
			_discord.Dispose();
		}

		public async Task Initialize()
		{
			await _discord.InitializeAsync();
		}

		public ulong UserId
		{
			get { return _discord.CurrentUser.Id; }
		}

		public bool IsGlobalAdministrator()
		{
			// TODO check database for this
			return UserId == 191587255557554177u; // Oppzippy#2963
		}

		public async Task<bool> IsOrganizationAdministrator(BackedOrganization organization)
		{
			DiscordMember member = await GetCurrentUserGuildMember(organization.DiscordGuildId);
			return IsGlobalAdministrator() || member.IsOwner || member.Permissions.HasPermission(Permissions.Administrator);
		}

		public async Task<bool> IsOrganizationModerator(BackedOrganization organization)
		{
			DiscordMember member = await GetCurrentUserGuildMember(organization.DiscordGuildId);
			return IsGlobalAdministrator() || member.IsOwner || member.Permissions.HasPermission(Permissions.ManageGuild);
		}

		public async Task<bool> IsOrganizationMember(BackedOrganization organization)
		{
			DiscordMember member = await GetCurrentUserGuildMember(organization.DiscordGuildId);
			return IsGlobalAdministrator() || member != null;
		}

		private async Task<DiscordMember> GetCurrentUserGuildMember(ulong guildId)
		{
			DiscordMember member;
			if (_currentUserMemberCache.TryGetValue(guildId, out member))
			{
				return member;
			}
			try
			{
				member = await _discord.GetGuildMemberAsync(guildId, UserId);
			}
			catch (NotFoundException) { }
			_currentUserMemberCache[guildId] = member;
			return member;
		}
	}
}
