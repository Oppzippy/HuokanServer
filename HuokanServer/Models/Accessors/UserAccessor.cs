using HuokanServer.Models.Repository.UserRepository;
using Microsoft.AspNetCore.Http;

namespace HuokanServer.Models.Accessors
{
	public class UserAccessor : IUserAccessor
	{
		public BackedUser User
		{
			get
			{
				return _httpContextAccessor.HttpContext.Features.Get<BackedUser>();
			}
		}

		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserAccessor(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
	}
}
