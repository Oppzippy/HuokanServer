using System.Data;
using System.Net.Http;
using DbUp;
using DbUp.Engine;
using HuokanServer.Authentication;
using HuokanServer.Authorization;
using HuokanServer.Middleware;
using HuokanServer.Models.Discord;
using HuokanServer.Models.OAuth2;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.UserPermissionRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;

namespace HuokanServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			string connectionString = Configuration.GetConnectionString("Postgres");
			Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
			EnsureDatabase.For.PostgresqlDatabase(connectionString);
			UpgradeEngine upgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
				.WithScriptsEmbeddedInAssembly(typeof(HuokanServer.Program).Assembly)
				.LogToConsole()
				.Build();
			upgrader.PerformUpgrade();

			services.AddTransient<HttpClient>();
			services.AddHttpContextAccessor();

			IConfigurationSection applicationSettings = Configuration.GetSection("Application");
			services.AddSingleton<ApplicationSettings>(new ApplicationSettings()
			{
				DbConnectionString = connectionString,
				BaseUrl = applicationSettings["BaseUrl"],
				DiscordClientId = applicationSettings["DiscordClientId"],
				DiscordClientSecret = applicationSettings["DiscordClientSecret"],
				DiscordRedirectUrl = applicationSettings["BaseUrl"] + "/authorization/discord/authorize",
			});
			services.AddModels();

			services.AddAuthentication("ApiKey")
				.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
			services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
			services.AddAuthorization(options =>
			{
				options.AddPolicy("OrganizationAdministrator", policy => policy.AddRequirements(new PermissionRequirement(Permission.ORGANIZATION_ADMINISTRATOR)));
				options.AddPolicy("OrganizationModerator", policy => policy.AddRequirements(new PermissionRequirement(Permission.ORGANIZATION_MODERATOR)));
				options.AddPolicy("OrganizationMember", policy => policy.AddRequirements(new PermissionRequirement(Permission.ORGANIZATION_MEMBER)));
				options.AddPolicy("User", policy => policy.AddRequirements(new PermissionRequirement(Permission.USER)));
			});

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "HuokanServer", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HuokanServer v1"));
			}

			app.UseRouting();
			// app.UseMiddleware<ApiKeyMiddleware>();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
