using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace HuokanServer.Models.Repository.OrganizationRepository
{
	public class OrganizationRepository : RepositoryBase
	{
		public OrganizationRepository(IDbConnection dbConnection) : base(dbConnection) { }

		public async Task<BackedOrganization> FindOrganization(string slug)
		{
			return await dbConnection.QueryFirstAsync<BackedOrganization>(@"
				SELECT
					id,
					'name',
					slug,
					created_at
				FROM
					organization
				WHERE
					slug = @Slug",
				new
				{
					Slug = slug,
				}
			);
		}

		public async Task CreateOrganization(Organization organization)
		{
			await dbConnection.ExecuteAsync(@"
				INSERT INTO organization ('name', slug, created_at)
				VALUES (@Name, @Slug, @CreatedAt)",
				new
				{
					Name = organization.Name,
					Slug = organization.Slug,
					CreatedAt = DateTime.UtcNow,
				}
			);
		}
	}
}
