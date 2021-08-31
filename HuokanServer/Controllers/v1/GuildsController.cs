using System.Collections.Generic;
using System.Linq;
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
		public async Task<IEnumerable<GuildResponse>> GetGuilds([FromQuery(Name = "name")] string guildName, [FromQuery(Name = "realm")] string guildRealm)
		{
			List<BackedGuild> guilds = await _guildRepository.FindGuilds(new Guild()
			{
				OrganizationId = _user.OrganizationId,
				Name = guildName,
				Realm = guildRealm,
			});
			return guilds.Select(guild => new GuildResponse()
			{
				Name = guild.Name,
				Realm = guild.Realm,
			});
		}

		[HttpPost]
		[Route("/guilds")]
		public async Task<GuildResponse> CreateGuild()
		{

		}

		public record GuildResponse
		{
			public string Name { get; init; }
			public string Realm { get; init; }
		}
	}
}
