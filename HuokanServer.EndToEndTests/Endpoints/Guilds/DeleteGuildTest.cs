using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.EndToEndTests.TestBases;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class DeleteGuildTest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestDeleteGuild()
		{
			HttpResponseMessage response = await HttpClient.DeleteAsync($"{BaseUrl}/organizations/{Organization.Id}/guilds/{Guild.Id}");
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			var guildRepository = new GuildRepository(ConnectionFactory);
			await Assert.ThrowsAnyAsync<ItemNotFoundException>(() => guildRepository.GetGuild(Organization.Id, Guild.Id));
		}
	}
}
