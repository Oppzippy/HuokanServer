using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace HuokanServer.DataAccess.Cache.DiscordGuildMember;

public class DiscordGuildMemberCache
{
	private readonly IConnectionMultiplexer _redis;

	public DiscordGuildMemberCache(IConnectionMultiplexer redis)
	{
		_redis = redis;
	}

	public async Task<HuokanServer.DataAccess.Discord.Bot.DiscordGuildMember> GetGuildMember(ulong guildId, ulong userId)
	{
		IDatabase database = _redis.GetDatabase();
		Task<bool?> isOwner = IsOwner(database, guildId, userId);
		Task<string> username = GetUsername(database, guildId, userId);
		Task<string> discriminator = GetDiscriminator(database, guildId, userId);
		Task<long?> permissions = GetPermissions(database, guildId, userId);
		Task<ISet<ulong>> roleIds = GetRoleIds(database, guildId, userId);

		await Task.WhenAll(isOwner, username, discriminator, permissions, roleIds);

		if (isOwner.Result != null && username.Result != null && discriminator.Result != null && permissions.Result != null && roleIds.Result != null)
		{
			return new HuokanServer.DataAccess.Discord.Bot.DiscordGuildMember()
			{
				Id = userId,
				IsOwner = (bool)isOwner.Result,
				Username = username.Result,
				Discriminator = discriminator.Result,
				Permissions = (long)permissions.Result,
				RoleIds = roleIds.Result,
			};
		}
		return null;
	}

	public async Task SetGuildMember(ulong guildId, ulong userId, HuokanServer.DataAccess.Discord.Bot.DiscordGuildMember discordGuildMember)
	{
		ITransaction transaction = _redis.GetDatabase().CreateTransaction();
		_ = SetIsOwner(transaction, guildId, userId, discordGuildMember.IsOwner);
		_ = SetUsername(transaction, guildId, userId, discordGuildMember.Username);
		_ = SetDiscriminator(transaction, guildId, userId, discordGuildMember.Discriminator);
		_ = SetPermissions(transaction, guildId, userId, discordGuildMember.Permissions);
		_ = SetRoleIds(transaction, guildId, userId, discordGuildMember.RoleIds);
		await transaction.ExecuteAsync();
	}

	private async Task<bool?> IsOwner(IDatabaseAsync database, ulong guildId, ulong userId)
	{
		string isOwner = await database.StringGetAsync($"{GetGuildMemberKey(guildId, userId)}:IsOwner");
		if (isOwner == null)
		{
			return null;
		}
		return isOwner == "true";
	}

	private async Task SetIsOwner(IDatabaseAsync database, ulong guildId, ulong userId, bool isOwner)
	{
		await database.StringSetAsync(
			$"{GetGuildMemberKey(guildId, userId)}:IsOwner",
			isOwner ? "true" : "false",
			TimeSpan.FromMinutes(5)
		);
	}

	private async Task<string> GetUsername(IDatabaseAsync database, ulong guildId, ulong userId)
	{
		return await database.StringGetAsync($"{GetGuildMemberKey(guildId, userId)}:Username");
	}

	private async Task SetUsername(IDatabaseAsync database, ulong guildId, ulong userId, string username)
	{
		await database.StringSetAsync(
			$"{GetGuildMemberKey(guildId, userId)}:Username",
			username,
			TimeSpan.FromMinutes(5)
		);
	}

	private async Task<string> GetDiscriminator(IDatabaseAsync database, ulong guildId, ulong userId)
	{
		return await database.StringGetAsync($"{GetGuildMemberKey(guildId, userId)}:Discriminator");
	}

	private async Task SetDiscriminator(IDatabaseAsync database, ulong guildId, ulong userId, string discriminator)
	{
		await database.StringSetAsync(
			$"{GetGuildMemberKey(guildId, userId)}:Discriminator",
			discriminator,
			TimeSpan.FromMinutes(5)
		);
	}

	private async Task<long?> GetPermissions(IDatabaseAsync database, ulong guildId, ulong userId)
	{
		return (long?)await database.StringGetAsync($"{GetGuildMemberKey(guildId, userId)}:Permissions");
	}

	private async Task SetPermissions(IDatabaseAsync database, ulong guildId, ulong userId, long permissions)
	{
		await database.StringSetAsync(
			$"{GetGuildMemberKey(guildId, userId)}:Permissions",
			permissions,
			TimeSpan.FromMinutes(5)
		);
	}

	private async Task<ISet<ulong>> GetRoleIds(IDatabaseAsync database, ulong guildId, ulong userId)
	{
		RedisValue[] roleIds = await database.SetMembersAsync($"{GetGuildMemberKey(guildId, userId)}:RoleIds");
		if (roleIds != null)
		{
			return roleIds.Select(roleId => ((ulong)roleId)).ToHashSet();
		}
		return null;
	}

	private async Task SetRoleIds(IDatabaseAsync database, ulong guildId, ulong userId, IEnumerable<ulong> roleIds)
	{
		var key = $"{GetGuildMemberKey(guildId, userId)}:RoleIds";
		await database.SetAddAsync(
			key,
			roleIds.Select(roleId => new RedisValue(roleId.ToString())).ToArray()
		);
		await database.KeyExpireAsync(key, TimeSpan.FromMinutes(5));
	}

	private string GetGuildMemberKey(ulong guildId, ulong userId)
	{
		return $"DiscordGuildMember:{guildId}:{userId}";
	}
}
