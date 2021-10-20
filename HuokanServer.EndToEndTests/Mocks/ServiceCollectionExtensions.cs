using HuokanServer.DataAccess.Discord.User;
using HuokanServer.DataAccess.Repository.UserPermissionRepository;
using HuokanServer.EndToEndTests.Mocks.DataAccess.Discord;
using HuokanServer.EndToEndTests.Mocks.DataAccess.Repository.UserPermissionRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HuokanServer.EndToEndTests.Mocks
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMocks(this IServiceCollection services)
		{
			services.RemoveAll<IOrganizationUserPermissionRepository>();
			services.RemoveAll<IDiscordUserFactory>();
			services.AddTransient<IDiscordUserFactory, DiscordUserFactoryMock>();
			services.AddTransient<IOrganizationUserPermissionRepository, OrganizationUserPermissionRepositoryMock>();
			return services;
		}
	}
}
