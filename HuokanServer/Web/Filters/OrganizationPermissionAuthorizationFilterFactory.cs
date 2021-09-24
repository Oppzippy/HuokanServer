using System;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HuokanServer.Web.Filters
{
	public class OrganizationPermissionAuthorizationFilterFactoryAttribute : Attribute, IFilterFactory
	{
		public OrganizationPermission RequiredPermission { get; set; }
		public bool IsReusable => false;

		public OrganizationPermissionAuthorizationFilterFactoryAttribute(OrganizationPermission requiredPermission)
		{
			RequiredPermission = requiredPermission;
		}

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			var filter = serviceProvider.GetService<OrganizationPermissionAuthorizationFilter>();
			filter.RequiredPermission = RequiredPermission;
			return filter;
		}
	}
}
