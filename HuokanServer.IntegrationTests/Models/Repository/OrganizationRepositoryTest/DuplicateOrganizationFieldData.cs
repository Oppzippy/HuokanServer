using System.Collections;
using System.Collections.Generic;
using HuokanServer.Models.Repository.OrganizationRepository;

namespace HuokanServer.IntegrationTests.Models.Repository.OrganizationRepositoryTest
{
	public class OrganizationDuplicateFieldData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			yield return new object[] {
				new Organization(){
					DiscordGuildId = 1,
					Name = "Org1",
					Slug = "org1"
				},
				new Organization(){
					DiscordGuildId = 1,
					Name = "Org2",
					Slug = "org2",
				},
			};
			yield return new object[]{
				new Organization(){
					DiscordGuildId = 2,
					Name = "Org3",
					Slug = "org3",
				},
				new Organization(){
					DiscordGuildId = 3,
					Name = "org4",
					Slug = "org3",
				},
			};
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
