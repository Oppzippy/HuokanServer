using HuokanServer.Models.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace HuokanServer.Authorization
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
		{
			services.AddScoped<IAuthorizationHandler, OrganizationPermissionRequirementHandler>();
			services.AddScoped<IAuthorizationHandler, GlobalPermissionRequirementHandler>();

			services.AddAuthorization(options =>
			{
				options.AddPolicy("OrganizationAdministrator", policy => policy.AddRequirements(new OrganizationPermissionRequirement(OrganizationPermission.ORGANIZATION_ADMINISTRATOR)));
				options.AddPolicy("OrganizationModerator", policy => policy.AddRequirements(new OrganizationPermissionRequirement(OrganizationPermission.ORGANIZATION_MODERATOR)));
				options.AddPolicy("OrganizationMember", policy => policy.AddRequirements(new OrganizationPermissionRequirement(OrganizationPermission.ORGANIZATION_MEMBER)));
				options.AddPolicy("User", policy => policy.AddRequirements(new GlobalPermissionRequirement(GlobalPermission.USER)));
			});
			return services;
		}
	}
}
