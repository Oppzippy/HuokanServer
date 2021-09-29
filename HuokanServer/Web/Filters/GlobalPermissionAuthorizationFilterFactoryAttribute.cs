using System;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HuokanServer.Web.Filters
{
	public class GlobalPermissionAuthorizationFilterFactoryAttribute : Attribute, IFilterFactory
	{
		public GlobalPermission RequiredPermission { get; set; }
		public bool IsReusable => false;

		public GlobalPermissionAuthorizationFilterFactoryAttribute(GlobalPermission requiredPermission)
		{
			RequiredPermission = requiredPermission;
		}

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			var filter = serviceProvider.GetService<GlobalPermissionAuthorizationFilter>();
			filter.RequiredPermission = RequiredPermission;
			return filter;
		}
	}
}
