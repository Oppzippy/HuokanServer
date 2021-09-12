using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers.v1.Organizations.Guilds
{
	[ApiController]
	[Route("organizations/{organizationId}/guilds")]
	public class GuildsController : ControllerBase
	{
		private IGuildRepository _guildRepository;
		private BackedUser _user;

		public GuildsController(IGuildRepository guildRepository)
		{
			_guildRepository = guildRepository;
			_user = HttpContext.Features.Get<BackedUser>();
		}

		[HttpGet]
		[Authorize(Policy = "OrganizationMember")]
		public async Task<IEnumerable<ApiGuild>> GetGuilds(
			[FromRoute(Name = "organizationId")] Guid organizationId,
			[FromQuery(Name = "name")] string guildName,
			[FromQuery(Name = "realm")] string guildRealm
		)
		{
			List<BackedGuild> guilds = await _guildRepository.FindGuilds(organizationId, new GuildFilter()
			{
				Name = guildName,
				Realm = guildRealm,
			});
			return guilds.Select(ApiGuild.From);
		}

		[HttpGet]
		[Route("{guildId}")]
		[Authorize(Policy = "OrganizationMember")]
		public async Task<ApiGuild> GetGuild(
			[FromRoute(Name = "organizationId")] Guid organizationId,
			[FromRoute(Name = "guildID")] Guid guildId
		)
		{
			BackedGuild guild = await _guildRepository.GetGuild(organizationId, guildId);
			return ApiGuild.From(guild);
		}

		[HttpPost]
		[Authorize(Policy = "OrganizationAdministrator")]
		public async Task<ApiGuild> CreateGuild([FromRoute(Name = "organizationId")] Guid organizationId, [FromBody] ApiGuild guildInfo)
		{
			BackedGuild newGuild = await _guildRepository.CreateGuild(new Guild()
			{
				OrganizationId = organizationId,
				Name = guildInfo.Name,
				Realm = guildInfo.Realm,
			});
			return ApiGuild.From(newGuild);
		}

		[HttpPatch]
		[Route("{guildId}")]
		[Authorize(Policy = "OrganizationAdministrator")]
		public async Task<ApiGuild> UpdateGuild(
			[FromRoute(Name = "organizationId")] Guid organizationId,
			[FromRoute(Name = "guildId")] Guid guildId,
			[FromBody] ApiGuild guildInfo
		)
		{
			BackedGuild guild = await _guildRepository.GetGuild(organizationId, guildId);
			BackedGuild modifiedGuild = guild with
			{
				Name = guildInfo.Name,
				Realm = guildInfo.Realm,
			};
			BackedGuild updatedGuild = await _guildRepository.UpdateGuild(modifiedGuild);
			return ApiGuild.From(updatedGuild);
		}

		[HttpDelete]
		[Route("{guildId}")]
		[Authorize(Policy = "OrganizationAdministrator")]
		public async Task DeleteGuild([FromRoute(Name = "organizationId")] Guid organizationId, [FromRoute(Name = "guildId")] Guid guildId)
		{
			await _guildRepository.DeleteGuild(organizationId, guildId);
		}
	}
}
