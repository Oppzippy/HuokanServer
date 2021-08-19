using System.Data;

namespace HuokanServer.Models.Repository
{
	public abstract class RepositoryBase
	{
		protected IDbConnection dbConnection;
		public RepositoryBase(IDbConnection dbConnection)
		{
			this.dbConnection = dbConnection;
		}
	}
}
