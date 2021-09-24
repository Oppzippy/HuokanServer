using System.Net.Http;
using DbUp;
using DbUp.Engine;
using HuokanServer.DataAccess.Repository;
using HuokanServer.Web.Authentication;
using HuokanServer.Web.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
			RunDatabaseMigrations();

			services.AddApplicationSettings(Configuration);
			services.AddDataAccess();
			services.AddTransient<HttpClient>();
			services.AddHttpContextAccessor();

			services.AddAuthentication("ApiKey")
				.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
			services.AddAuthorizationPolicies();

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "HuokanServer", Version = "v1" });
			});
		}

		private void RunDatabaseMigrations()
		{
			string connectionString = Configuration.GetConnectionString("Postgres");
			Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
			EnsureDatabase.For.PostgresqlDatabase(connectionString);
			UpgradeEngine upgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
				.WithScriptsEmbeddedInAssembly(typeof(HuokanServer.Program).Assembly)
				.LogToConsole()
				.Build();
			upgrader.PerformUpgrade();
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
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
