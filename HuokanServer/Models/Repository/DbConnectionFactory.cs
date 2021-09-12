using System.Data;
using Npgsql;

namespace HuokanServer.Models.Repository
{
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
			return new NpgsqlConnection(ConnectionString);
		}
	}
}
