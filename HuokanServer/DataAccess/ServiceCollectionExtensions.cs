using HuokanServer.DataAccess.Cache.DiscordGuildMember;
using HuokanServer.DataAccess.Cache.DiscordUser;
using HuokanServer.DataAccess.Discord.Bot;
using HuokanServer.DataAccess.Discord.User;
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
using StackExchange.Redis;

namespace HuokanServer.DataAccess.Repository;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDataAccess(this IServiceCollection services)
	{
		AddDatabaseRepositories(services);
		AddDiscord(services);
		AddCache(services);
		AddOAuth(services);
		AddPermissions(services);

		return services;
	}

	private static void AddDatabaseRepositories(IServiceCollection services)
	{
		services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
		services.AddTransient<IApiKeyRepository, ApiKeyRepository.ApiKeyRepository>();
		services.AddTransient<IDepositRepository, DepositRepository.DepositRepository>();
		services.AddTransient<IDepositImportExecutorFactory, DepositImportExecutorFactory>();
		services.AddTransient<IGuildRepository, GuildRepository.GuildRepository>();
		services.AddTransient<IOrganizationRepository, OrganizationRepository.OrganizationRepository>();
		services.AddTransient<IUserDiscordTokenRepository, UserDiscordTokenRepository.UserDiscordTokenRepository>();
		services.AddTransient<IGlobalUserPermissionRepository, GlobalUserPermissionRepository>();
		services.AddTransient<IOrganizationUserPermissionRepository, OrganizationUserPermissionRepository>();
		services.AddTransient<IUserRepository, UserRepository.UserRepository>();
	}

	private static void AddDiscord(IServiceCollection services)
	{
		// User
		services.AddTransient<IUnknownDiscordUserFactory, UnknownDiscordUserFactory>();
		services.AddTransient<IKnownDiscordUserFactory, CachedDiscordUserFactory>();
		services.AddTransient<IDiscordUserAuthenticationHandlerFactory, DiscordUserAuthenticationHandlerFactory>();
		// Bot
		services.AddTransient<DiscordBot>();
		services.AddTransient<IDiscordBot>(provider =>
		{
			var bot = provider.GetService<DiscordBot>();
			return new CachedDiscordBot(provider.GetService<DiscordGuildMemberCache>(), bot);
		});
	}

	private static void AddCache(IServiceCollection services)
	{
		// TODO configure
		var multiplexer = ConnectionMultiplexer.Connect("localhost");
		services.AddSingleton<IConnectionMultiplexer>(multiplexer);
		services.AddTransient<DiscordGuildMemberCache>();
		services.AddTransient<CachedDiscordUserFactory>();
		services.AddTransient<DiscordUserCache>();
	}

	private static void AddOAuth(IServiceCollection services)
	{
		services.AddTransient<IOAuth2Factory, OAuth2Factory>();
	}

	private static void AddPermissions(IServiceCollection services)
	{
		services.AddTransient<IPermissionResolver, PermissionResolver>();
	}
}
