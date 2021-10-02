using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Users
{
	[ApiController]
	[Route("users")]
	public class UsersController : LoggedInControllerBase
	{
		[HttpGet]
		[Route("me")]
		[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.USER)]
		public UserModel GetMe()
		{
			return UserModel.From(User);
		}
	}
}
