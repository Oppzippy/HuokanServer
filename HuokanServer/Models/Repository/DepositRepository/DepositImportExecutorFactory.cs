namespace HuokanServer.Models.Repository.DepositRepository
{
	public class DepositImportExecutorFactory : IDepositImportExecutorFactory
	{
		private readonly IDbConnectionFactory _connectionFactory;

		public DepositImportExecutorFactory(IDbConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public IDepositImportExecutor Create()
		{
			return new DepositImportExecutor(_connectionFactory);
		}
	}
}
