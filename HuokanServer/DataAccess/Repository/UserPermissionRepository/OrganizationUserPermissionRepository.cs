using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord.Bot;
using HuokanServer.DataAccess.Discord.User;
using HuokanServer.DataAccess.Repository.OrganizationRepository;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository;

public class OrganizationUserPermissionRepository : IOrganizationUserPermissionRepository
{
	private readonly IKnownDiscordUserFactory _discordUserFactory;
	private readonly IDiscordBot _discordBot;

	public OrganizationUserPermissionRepository(IDiscordBot discordBot, IKnownDiscordUserFactory discordUserFactory)
	{
		_discordUserFactory = discordUserFactory;
		_discordBot = discordBot;
	}

	public async Task<bool> IsAdministrator(BackedOrganization organization, Guid huokanUserId, ulong userId)
	{
		DiscordGuildMember member = await GetDiscordGuildMemberAsync(organization.DiscordGuildId, userId);
		return member.IsOwner || member.HasPermission((long)DSharpPlus.Permissions.Administrator);
	}

	public async Task<bool> IsModerator(BackedOrganization organization, Guid huokanUserId, ulong userId)
	{
		DiscordGuildMember member = await GetDiscordGuildMemberAsync(organization.DiscordGuildId, userId);
		return member.IsOwner || member.HasPermission((long)DSharpPlus.Permissions.ManageGuild);
	}

	public async Task<bool> IsMember(BackedOrganization organization, Guid huokanUserId, ulong userId)
	{
		DiscordGuildMember member = await GetDiscordGuildMemberAsync(organization.DiscordGuildId, userId);
		return member != null;
	}

	private async Task<DiscordGuildMember> GetDiscordGuildMemberAsync(ulong guildId, ulong userId, Guid huokanUserId)
	{
		IDiscordUser discordUser = await _discordUserFactory.Create(huokanUserId, userId);
		return await GetDiscordGuildMemberAsync(guildId, await discordUser.GetId());
	}

	private async Task<DiscordGuildMember> GetDiscordGuildMemberAsync(ulong guildId, ulong userId)
	{
		return await _discordBot.GetGuildMember(guildId, userId);
	}
}
