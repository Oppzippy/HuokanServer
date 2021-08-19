using System;
using System.Data;
using System.Reflection;
using Dapper;
using DbUp;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace HuokanServer.IntegrationTests
{
	public class DatabaseFixture : IDisposable
	{
		public IDbConnection DbConnection { get; }

		public DatabaseFixture()
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.IntegrationTest.json")
				.Build();
			var connectionString = config.GetConnectionString("Postgres");
			EnsureDatabase.For.PostgresqlDatabase(connectionString);
			var upgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
				.WithScriptsEmbeddedInAssembly(typeof(HuokanServer.Program).Assembly)
				.LogToConsole()
				.Build();
			upgrader.PerformUpgrade();
			DbConnection = new NpgsqlConnection(connectionString);
		}

		public void Dispose()
		{
			DbConnection.Execute("DROP DATABASE huokan");
			DbConnection.Dispose();
		}
	}
}
