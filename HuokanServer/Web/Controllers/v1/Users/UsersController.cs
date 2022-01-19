using System;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1.Users;

[ApiController]
[Route("users")]
public class UsersController : LoggedInControllerBase
{
	private readonly IUserRepository _userRepository;

	public UsersController(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	[HttpGet]
	[Route("me")]
	[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.USER)]
	public UserModel GetMe()
	{
		return UserModel.From(User);
	}

	[HttpGet]
	[Route("{userId:guid}")]
	[GlobalPermissionAuthorizationFilterFactory(GlobalPermission.ADMINISTRATOR)]
	public async Task<UserModel> GetUser([FromRoute(Name = "userId")] Guid userId)
	{
		BackedUser user = await _userRepository.GetUser(userId);
		return UserModel.From(user);
	}
}