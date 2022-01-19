using System.Data;
using Npgsql;

namespace HuokanServer.DataAccess.Repository;

public class DbConnectionFactory : IDbConnectionFactory
{
	public string ConnectionString { get; }

	public DbConnectionFactory(ApplicationSettings settings)
	{
		ConnectionString = settings.DbConnectionString;
	}

	public DbConnectionFactory(string connectionString)
	{
		ConnectionString = connectionString;
	}


	public IDbConnection Create()
	{
		NpgsqlConnection dbConnection = new NpgsqlConnection(ConnectionString);
		// TODO use OpenAsync?
		dbConnection.Open();
		return dbConnection;
	}
}