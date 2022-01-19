using System.Data;

namespace HuokanServer.DataAccess.Repository;

public interface IDbConnectionFactory
{
	string ConnectionString { get; }
	IDbConnection Create();
}
