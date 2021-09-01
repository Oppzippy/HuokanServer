using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers
{
	[ApiController]
	[Route("/guilds/[action]")]
	public class GuildsController : ControllerBase
	{
		private GuildRepository _guildRepository;
		private BackedUser _user;

		public GuildsController(GuildRepository guildRepository)
		{
			_guildRepository = guildRepository;
			_user = HttpContext.Features.Get<BackedUser>();
		}

		[HttpGet]
		[Route("/guilds")]
		public async Task<IEnumerable<GuildInfo>> GetGuilds([FromQuery(Name = "name")] string guildName, [FromQuery(Name = "realm")] string guildRealm)
		{
			List<BackedGuild> guilds = await _guildRepository.FindGuilds(new Guild()
			{
				OrganizationId = _user.OrganizationId,
				Name = guildName,
				Realm = guildRealm,
			});
			return guilds.Select(guild => new GuildInfo()
			{
				Name = guild.Name,
				Realm = guild.Realm,
			});
		}

		[HttpPost]
		[Route("/guilds")]
		public async Task<GuildInfo> CreateGuild([FromBody] GuildInfo guildInfo)
		{
			BackedGuild newGuild = await _guildRepository.CreateGuild(new Guild()
			{
				OrganizationId = _user.OrganizationId,
				Name = guildInfo.Name,
				Realm = guildInfo.Realm,
			});
			return new GuildInfo()
			{
				Name = newGuild.Name,
				Realm = newGuild.Realm,
			};
		}

		[HttpGet]
		[Route("/guilds/{guildId}")]
		public async Task<GuildInfo> GetGuild([FromRoute] Guid id)
		{

		}

		public record GuildInfo
		{
			public string Name { get; init; }
			public string Realm { get; init; }
		}
	}
}
