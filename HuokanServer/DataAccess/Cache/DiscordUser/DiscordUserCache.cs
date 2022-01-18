using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace HuokanServer.DataAccess.Cache.DiscordUser;

public class DiscordUserCache
{
	private readonly IConnectionMultiplexer _redis;

	public DiscordUserCache(IConnectionMultiplexer redis)
	{
		_redis = redis;
	}

	public async Task SetGuildIds(ulong userId, IEnumerable<ulong> guildIds)
	{
		IDatabase db = _redis.GetDatabase();

		string key = $"DiscordUser:{userId}:GuildIds";
		RedisValue[] redisValues = guildIds.Select(guildId => new RedisValue(guildId.ToString())).ToArray();

		ITransaction transaction = db.CreateTransaction();
		await transaction.SetAddAsync(key, redisValues);
		await transaction.KeyExpireAsync(key, TimeSpan.FromMinutes(1));
		await transaction.ExecuteAsync();
	}

	public async Task<List<ulong>> GetGuildIds(ulong userId)
	{
		IDatabase db = _redis.GetDatabase();
		RedisValue[] guildIds = await db.SetMembersAsync($"DiscordUser:{userId}:GuildIds");
		if (guildIds != null)
		{
			return guildIds.Select(guildId => Convert.ToUInt64(guildId)).ToList();
		}
		return null;
	}
}
