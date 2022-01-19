using System;
using System.Data;
using Dapper;
using DbUp;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace HuokanServer.IntegrationTests;

public class DatabaseFixture : IDisposable
{
	public IDbConnection DbConnection { get; }
	private string _connectionString;

	public DatabaseFixture()
	{
		// TODO move match names with underscores elsewhere?
		Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
		IConfiguration config = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.IntegrationTest.json")
			.Build();
		_connectionString = config.GetConnectionString("Postgres");

		DbConnection = new NpgsqlConnection(_connectionString);
		ResetDB();
	}

	public void ResetDB()
	{
		DbConnection.Execute("DROP SCHEMA IF EXISTS public CASCADE");
		DbConnection.Execute("CREATE SCHEMA public");


		EnsureDatabase.For.PostgresqlDatabase(_connectionString);
		var upgrader = DeployChanges.To.PostgresqlDatabase(_connectionString)
			.WithScriptsEmbeddedInAssembly(typeof(HuokanServer.Program).Assembly)
			.LogToConsole()
			.Build();
		upgrader.PerformUpgrade();
	}

	public void Dispose()
	{
		// DbConnection.Execute("DROP SCHEMA IF EXISTS public CASCADE");
		// DbConnection.Execute("CREATE SCHEMA public");
		DbConnection.Dispose();
	}
}
