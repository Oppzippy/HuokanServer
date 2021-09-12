using System.Data;
using Npgsql;

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
			return _dbConnectionFactory.Create();
		}
	}
}
