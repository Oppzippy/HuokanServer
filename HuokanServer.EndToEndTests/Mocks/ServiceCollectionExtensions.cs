using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Discord;
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
			services.AddTransient<IOrganizationUserPermissionRepositoryFactory, OrganizationUserPermissionRepositoryFactoryMock>();
			services.AddTransient<IDiscordUserFactory, DiscordUserFactoryMock>();
			return services;
		}
	}
}
