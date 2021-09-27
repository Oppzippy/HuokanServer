using HuokanServer.DataAccess.Discord;
using HuokanServer.DataAccess.OAuth2;
using HuokanServer.DataAccess.Permissions;
using HuokanServer.DataAccess.Repository.ApiKeyRepository;
using HuokanServer.DataAccess.Repository.DepositRepository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.DataAccess.Repository.OrganizationRepository;
using HuokanServer.DataAccess.Repository.UserDiscordTokenRepository;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.DataAccess.Repository.UserRepository;
using Microsoft.Extensions.DependencyInjection;

namespace HuokanServer.DataAccess.Repository
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDataAccess(this IServiceCollection services)
		{
			services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
			// Discord
			services.AddTransient<IDiscordUserFactory, DiscordUserFactory>();
			services.AddTransient<IDiscordUserAuthenticationHandler, DiscordUserAuthenticationHandler>();
			// OAuth2
			services.AddTransient<IOAuth2Factory, OAuth2Factory>();
			// Permissions
			services.AddTransient<IPermissionResolver, PermissionResolver>();
			// Repository
			services.AddTransient<IApiKeyRepository, ApiKeyRepository.ApiKeyRepository>();
			services.AddTransient<IDepositRepository, DepositRepository.DepositRepository>();
			services.AddTransient<IDepositImportExecutorFactory, DepositImportExecutorFactory>();
			services.AddTransient<IGuildRepository, GuildRepository.GuildRepository>();
			services.AddTransient<IOrganizationRepository, OrganizationRepository.OrganizationRepository>();
			services.AddTransient<IUserDiscordTokenRepository, UserDiscordTokenRepository.UserDiscordTokenRepository>();
			services.AddTransient<IGlobalUserPermissionRepository, GlobalUserPermissionRepository>();
			services.AddTransient<IOrganizationUserPermissionRepositoryFactory, OrganizationUserPermissionRepositoryFactory>();
			services.AddTransient<IUserRepository, UserRepository.UserRepository>();

			return services;
		}
	}
}
