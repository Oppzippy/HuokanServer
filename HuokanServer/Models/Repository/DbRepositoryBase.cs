using System.Data;

namespace HuokanServer.Models.Repository
{
	public abstract class DbRepositoryBase
	{
		private readonly IDbConnectionFactory _dbConnectionFactory;

		public DbRepositoryBase(IDbConnectionFactory dbConnectionFactory)
		{
			_dbConnectionFactory = dbConnectionFactory;
		}

		protected IDbConnection GetDbConnection()
		{
			IDbConnection connection = _dbConnectionFactory.Create();
			connection.Open();
			return connection;
		}
	}
}
