using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Organizations.Guilds
{
	[ApiController]
	[Route("organizations/{organizationId}/guilds")]
	public class GuildsController : LoggedInControllerBase
	{
		private IGuildRepository _guildRepository;

		public GuildsController(IGuildRepository guildRepository)
		{
			_guildRepository = guildRepository;
		}

		[HttpGet]
		[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.MEMBER)]
		public async Task<GuildCollectionModel> GetGuilds(
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
			return new GuildCollectionModel()
			{
				Guilds = guilds.Select(GuildModel.From).ToList(),
			};
		}

		[HttpGet]
		[Route("{guildId}")]
		[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.MEMBER)]
		public async Task<GuildModel> GetGuild(
			[FromRoute(Name = "organizationId")] Guid organizationId,
			[FromRoute(Name = "guildID")] Guid guildId
		)
		{
			BackedGuild guild = await _guildRepository.GetGuild(organizationId, guildId);
			return GuildModel.From(guild);
		}

		[HttpPost]
		[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.ADMINISTRATOR)]
		public async Task<GuildModel> CreateGuild([FromRoute(Name = "organizationId")] Guid organizationId, [FromBody] GuildModel guildInfo)
		{
			BackedGuild newGuild = await _guildRepository.CreateGuild(new Guild()
			{
				OrganizationId = organizationId,
				Name = guildInfo.Name,
				Realm = guildInfo.Realm,
			});
			return GuildModel.From(newGuild);
		}

		[HttpPatch]
		[Route("{guildId}")]
		[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.ADMINISTRATOR)]
		public async Task<GuildModel> UpdateGuild(
			[FromRoute(Name = "organizationId")] Guid organizationId,
			[FromRoute(Name = "guildId")] Guid guildId,
			[FromBody] GuildModel guildInfo
		)
		{
			BackedGuild guild = await _guildRepository.GetGuild(organizationId, guildId);
			BackedGuild modifiedGuild = guild with
			{
				Name = guildInfo.Name,
				Realm = guildInfo.Realm,
			};
			BackedGuild updatedGuild = await _guildRepository.UpdateGuild(modifiedGuild);
			return GuildModel.From(updatedGuild);
		}

		[HttpDelete]
		[Route("{guildId}")]
		[OrganizationPermissionAuthorizationFilterFactory(OrganizationPermission.ADMINISTRATOR)]
		public async Task DeleteGuild([FromRoute(Name = "organizationId")] Guid organizationId, [FromRoute(Name = "guildId")] Guid guildId)
		{
			await _guildRepository.DeleteGuild(organizationId, guildId);
		}
	}
}
