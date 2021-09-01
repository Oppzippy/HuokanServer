using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
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
		[Authorize(Policy = "OrganizationMember")]
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

		[HttpGet]
		[Route("/guilds/{guildId}")]
		[Authorize(Policy = "OrganizationMember")]
		public async Task<GuildInfo> GetGuild([FromRoute] Guid id)
		{

		}

		[HttpPost]
		[Route("/guilds")]
		[Authorize(Policy = "OrganizationAdmin")]
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

		public record GuildInfo
		{
			public string Name { get; init; }
			public string Realm { get; init; }
		}
	}
}
