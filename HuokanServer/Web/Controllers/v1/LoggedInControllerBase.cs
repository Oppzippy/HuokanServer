using HuokanServer.DataAccess.Repository.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace HuokanServer.Web.Controllers.v1
{
	public class LoggedInControllerBase : ControllerBase
	{
		public new BackedUser User
		{
			get
			{
				return HttpContext.Features.Get<BackedUser>();
			}
		}
	}
}
