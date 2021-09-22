using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HuokanServer
{
	public record ApplicationSettings
	{
		public string DiscordClientId { get; init; }
		public string DiscordClientSecret { get; init; }
		public string BaseUrl { get; init; }
		public string DiscordRedirectUrl { get; set; }
		public string DbConnectionString { get; init; }
	}

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
		{
			IConfigurationSection applicationSettings = configuration.GetSection("Application");

			services.AddSingleton<ApplicationSettings>(new ApplicationSettings()
			{
				DbConnectionString = configuration.GetConnectionString("Postgres"),
				BaseUrl = applicationSettings["BaseUrl"],
				DiscordClientId = applicationSettings["DiscordClientId"],
				DiscordClientSecret = applicationSettings["DiscordClientSecret"],
				DiscordRedirectUrl = applicationSettings["BaseUrl"] + "/authorization/discord/authorize",
			});
			return services;
		}
	}
}
