using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using DbUp;
using DbUp.Engine;
using DSharpPlus;
using HuokanServer.DataAccess.Repository;
using HuokanServer.Web.Filters;
using HuokanServer.Web.Json;
using HuokanServer.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
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
			services.AddHttpLogging((options) =>
			{
				options.LoggingFields = HttpLoggingFields.All;
			});
			services.AddDataAccess();
			services.AddSingleton<DiscordClient>((serviceProvider) =>
			{
				var applicationSettings = serviceProvider.GetService<ApplicationSettings>();
				var discordClient = new DiscordClient(new DiscordConfiguration()
				{
					Intents = DiscordIntents.Guilds,
					Token = applicationSettings.DiscordBotToken,
				});
				discordClient.ConnectAsync().Wait();
				return discordClient;
			});

			services.AddTransient<HttpClient>();
			services.AddHttpContextAccessor();
			services.AddFilters();
			services.AddTransient<ApiKeyAuthenticationMiddleware>();
			services.AddTransient<ItemNotFound404Middleware>();
			services.AddTransient<DuplicateItem429Middleware>();

			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
					options.JsonSerializerOptions.Converters.Add(new JsonUlongStringConverter());
					options.JsonSerializerOptions.Converters.Add(new JsonDateTimeOffsetStringConverter());
				});
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", builder =>
				{
					builder.AllowAnyOrigin();
					builder.AllowAnyHeader();
					builder.AllowAnyMethod();
				});
			});

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Huokan", Version = "v1" });
				options.CustomOperationIds(e => e.ActionDescriptor.RouteValues["action"]);
				options.CustomSchemaIds(schema => Regex.Replace(schema.Name, "Model$", ""));
				options.MapType<ulong>(() => new OpenApiSchema()
				{
					Type = "string",
					Format = "uint64",
					Minimum = 0,
				});

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

				options.AddSecurityDefinition("ApiKeyAuth", apiKeySecurityScheme);
				options.AddSecurityDefinition("BearerAuth", bearerSecurityScheme);

				string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
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
				app.UseCors("AllowAll");
			}

			app.UseHttpLogging();
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
