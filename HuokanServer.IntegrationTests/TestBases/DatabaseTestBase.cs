using System;
using System.Data;
using Dapper;
using DbUp;
using HuokanServer.DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HuokanServer.IntegrationTests.TestBases;

[Collection("Database")]
public abstract class DatabaseTestBase
{
	protected IDbConnectionFactory ConnectionFactory { get; }

	public DatabaseTestBase()
	{
		// TODO move match names with underscores elsewhere?
		Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
		IConfiguration config = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.Build();
		ConnectionFactory = new DbConnectionFactory(config.GetConnectionString("Postgres"));

		ResetDB();
	}

	private void ResetDB()
	{
		using IDbConnection dbConnection = GetDbConnection();
		dbConnection.Execute("DROP SCHEMA IF EXISTS public CASCADE");
		dbConnection.Execute("CREATE SCHEMA public");


		EnsureDatabase.For.PostgresqlDatabase(ConnectionFactory.ConnectionString);
		var upgrader = DeployChanges.To.PostgresqlDatabase(ConnectionFactory.ConnectionString)
			.WithScriptsEmbeddedInAssembly(typeof(HuokanServer.Program).Assembly)
			.LogToConsole()
			.Build();
		upgrader.PerformUpgrade();
	}

	protected IDbConnection GetDbConnection()
	{
		return ConnectionFactory.Create();
	}
}