using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.DataAccess.Repository.UserPermissionRepository;

public class GlobalUserPermissionRepository : DbRepositoryBase, IGlobalUserPermissionRepository
{
	public GlobalUserPermissionRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

	public async Task<bool> IsAdministrator(Guid userId)
	{
		using IDbConnection dbConnection = GetDbConnection();
		try
		{
			await dbConnection.QueryFirstAsync(@"
					SELECT
						1
					FROM
						user_account
					WHERE
						external_id = @UserId AND
						permission_level = @PermissionLevel::global_permission_level",
				new
				{
					UserId = userId,
					PermissionLevel = "ADMINISTRATOR",
				}
			);
		}
		catch (InvalidOperationException)
		{
			// No results
			return false;
		}
		return true;
	}

	public async Task SetIsAdministrator(Guid userId, bool isAdministrator)
	{
		using IDbConnection dbConnection = GetDbConnection();
		await dbConnection.ExecuteAsync(@"
				UPDATE
					user_account
				SET
					permission_level = @PermissionLevel::global_permission_level
				WHERE
					external_id = @UserId",
			new
			{
				PermissionLevel = isAdministrator ? "ADMINISTRATOR" : "USER",
				UserId = userId,
			}
		);
	}
}