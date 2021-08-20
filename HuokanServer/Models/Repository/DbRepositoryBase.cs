using System.Data;

namespace HuokanServer.Models.Repository
{
	public abstract class DbRepositoryBase
	{
		protected readonly IDbConnection dbConnection;
		public DbRepositoryBase(IDbConnection dbConnection)
		{
			this.dbConnection = dbConnection;
		}
	}
}
