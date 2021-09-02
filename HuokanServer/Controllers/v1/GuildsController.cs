using System;
using System.Collections.Generic;
using System.Linq;
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
				Id = guild.Id,
				Name = guild.Name,
				Realm = guild.Realm,
			});
		}

		[HttpGet]
		[Route("/guilds/{guildId}")]
		[Authorize(Policy = "OrganizationMember")]
		public async Task<GuildInfo> GetGuild([FromRoute] Guid id)
		{
			BackedGuild guild = await _guildRepository.GetGuild(_user.OrganizationId, id);
			return new GuildInfo()
			{
				Id = guild.Id,
				Name = guild.Name,
				Realm = guild.Realm,
			};
		}

		[HttpPost]
		[Route("/guilds")]
		[Authorize(Policy = "OrganizationAdministrator")]
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
				Id = newGuild.Id,
				Name = newGuild.Name,
				Realm = newGuild.Realm,
			};
		}

		[HttpPatch]
		[Route("/guilds/{guildId}")]
		[Authorize(Policy = "OrganizationAdministrator")]
		public async Task<GuildInfo> UpdateGuild([FromRoute] Guid guildId, [FromBody] GuildInfo guildInfo)
		{
			BackedGuild guild = await _guildRepository.GetGuild(_user.OrganizationId, guildId);
			BackedGuild modifiedGuild = guild with
			{
				Name = guildInfo.Name,
				Realm = guildInfo.Realm,
			};
			BackedGuild updatedGuild = await _guildRepository.UpdateGuild(modifiedGuild);
			return new GuildInfo()
			{
				Id = updatedGuild.Id,
				Name = updatedGuild.Name,
				Realm = updatedGuild.Realm,
			};
		}

		[HttpDelete]
		[Route("/guilds/{guildId}")]
		[Authorize(Policy = "OrganizationAdministrator")]
		public async Task DeleteGuild([FromRoute] Guid guildId)
		{
			BackedGuild guild = await _guildRepository.GetGuild(_user.OrganizationId, guildId);
			await _guildRepository.DeleteGuild(guild);
		}

		public record GuildInfo
		{
			public Guid Id { get; init; }
			public string Name { get; init; }
			public string Realm { get; init; }
		}
	}
}
