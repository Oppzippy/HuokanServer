using System.Collections.Generic;
using System.Linq;
using DSharpPlus.Entities;

namespace HuokanServer.DataAccess.Discord.Bot;

public record DiscordGuildMember
{
	public ulong Id { get; init; }
	public bool IsOwner { get; init; }
	public string Username { get; init; }
	public string Discriminator { get; init; }
	public long Permissions { get; init; }
	public ISet<ulong> RoleIds { get; init; }

	public static DiscordGuildMember FromDSharpPlus(DiscordMember member)
	{
		return new DiscordGuildMember()
		{
			Id = member.Id,
			IsOwner = member.IsOwner,
			Username = member.Username,
			Discriminator = member.Discriminator,
			Permissions = (long)member.Permissions,
			RoleIds = member.Roles.Select(role => role.Id).ToHashSet(),
		};
	}

	public bool HasPermission(long permission)
	{
		return (Permissions & permission) != 0 || IsOwner;
	}
}
