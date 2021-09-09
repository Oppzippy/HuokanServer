using System.Data;
using System.Net.Http;
using HuokanServer.Authorization;
using HuokanServer.Middleware;
using HuokanServer.Models.Discord;
using HuokanServer.Models.OAuth2;
using HuokanServer.Models.Repository;
using HuokanServer.Models.Repository.UserPermissionRepository;
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
			var connectionString = Configuration.GetConnectionString("Postgres");
			// TODO make sure connection pooling is working
			services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(connectionString));
			services.AddTransient<HttpClient>();
			services.AddSingleton<ApplicationSettings>(new ApplicationSettings()
			{
				DiscordClientId = "todo",
				DiscordClientSecret = "todo",
				DiscordRedirectUri = "todo",
			});
			services.AddModels();

			services.AddAuthorization(options =>
			{
				options.AddPolicy("OrganizationAdministrator", policy => policy.AddRequirements(new PermissionRequirement(Permission.ORGANIZATION_ADMINISTRATOR)));
				options.AddPolicy("OrganizationModerator", policy => policy.AddRequirements(new PermissionRequirement(Permission.ORGANIZATION_MODERATOR)));
				options.AddPolicy("OrganizationMember", policy => policy.AddRequirements(new PermissionRequirement(Permission.ORGANIZATION_MEMBER)));
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
			app.UseMiddleware<ApiKeyMiddleware>();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
