using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Controllers.Auth
{
	[ApiController]
	[Route("/v1/authorization/discord/authorize")]
	public class DiscordAuthorizationController : ControllerBase
	{
		public DiscordAuthorizationController()
		{

		}

		[HttpGet]
		public void RedirectToDiscord()
		{

		}
	}
}
