using HuokanServer.Models.Discord;
using HuokanServer.Models.OAuth2;
using HuokanServer.Models.Permissions;
using HuokanServer.Models.Repository.ApiKeyRepository;
using HuokanServer.Models.Repository.DepositRepository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.OrganizationRepository;
using HuokanServer.Models.Repository.UserDiscordTokenRepository;
using HuokanServer.Models.Repository.UserPermissionRepository;
using HuokanServer.Models.Repository.UserRepository;
using Microsoft.Extensions.DependencyInjection;

namespace HuokanServer.Models.Repository
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddModels(this IServiceCollection services)
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
			services.AddTransient<IUserPermissionRepositoryFactory, UserPermissionRepositoryFactory>();
			services.AddTransient<IUserRepository, UserRepository.UserRepository>();

			return services;
		}
	}
}
