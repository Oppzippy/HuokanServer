using System.Net;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using Microsoft.AspNetCore.Http;

namespace HuokanServer.Web.Middleware
{
	public class ItemNotFound404Middleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (ItemNotFoundException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
		}
	}
}
