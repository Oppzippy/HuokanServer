using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using DbUp;
using DbUp.Engine;
using HuokanServer.DataAccess.Repository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Middleware;
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
		public virtual void ConfigureServices(IServiceCollection services)
		{
			RunDatabaseMigrations();

			services.AddApplicationSettings(Configuration);
			services.AddDataAccess();
			services.AddTransient<HttpClient>();
			services.AddHttpContextAccessor();
			services.AddFilters();
			services.AddTransient<ApiKeyAuthenticationMiddleware>();
			services.AddTransient<ItemNotFound404Middleware>();
			services.AddTransient<DuplicateItem429Middleware>();

			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Huokan", Version = "v1" });
				c.CustomOperationIds(e => e.ActionDescriptor.RouteValues["action"]);
				c.CustomSchemaIds(schema => Regex.Replace(schema.Name, "Model$", ""));

				var apiKeySecurityScheme = new OpenApiSecurityScheme()
				{
					Name = "X-API-Key",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
				};
				var bearerSecurityScheme = new OpenApiSecurityScheme()
				{
					Description = "Provided as an alternative to X-API-Key for SDK generators that don't properly support custom headers for auth.",
					Name = "Authorization",
					Scheme = "Bearer",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
				};

				c.AddSecurityDefinition("ApiKeyAuth", apiKeySecurityScheme);
				c.AddSecurityDefinition("BearerAuth", bearerSecurityScheme);

				string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
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
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Huokan v1"));
			}

			app.UseRouting();
			app.UseMiddleware<ApiKeyAuthenticationMiddleware>();
			app.UseMiddleware<ItemNotFound404Middleware>();
			app.UseMiddleware<DuplicateItem429Middleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
