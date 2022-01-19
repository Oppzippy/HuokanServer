using Microsoft.Extensions.DependencyInjection;

namespace HuokanServer.Web.Filters;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddFilters(this IServiceCollection services)
	{
		services.AddTransient<GlobalPermissionAuthorizationFilter>();
		services.AddTransient<OrganizationPermissionAuthorizationFilter>();
		return services;
	}
}
