using System.Data;

namespace HuokanServer.Models.Repository
{
	public interface IDbConnectionFactory
	{
		string ConnectionString { get; }
		IDbConnection Create();
	}
}
