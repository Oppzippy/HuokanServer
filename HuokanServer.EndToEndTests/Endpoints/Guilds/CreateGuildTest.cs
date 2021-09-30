using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HuokanServer.DataAccess.Repository.GuildRepository;
using HuokanServer.EndToEndTests.TestBases;
using HuokanServer.Web.Models;
using Xunit;

namespace HuokanServer.EndToEndTests.Endpoints.Guilds
{
	public class CreateGuildtest : OrganizationUserTestBase
	{
		[Fact]
		public async Task TestCreateGuild()
		{
			HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
				$"{BaseUrl}/organizations/{Organization.Id}/guilds",
				new GuildModel()
				{
					Name = "Test Guild",
					Realm = "TestRealm",
				}
			);
			var guild = await response.Content.ReadFromJsonAsync<GuildModel>();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(guild.Id);
			Assert.Equal("Test Guild", guild.Name);
			Assert.Equal("TestRealm", guild.Realm);

			var guildRepository = new GuildRepository(ConnectionFactory);
			await guildRepository.GetGuild(Organization.Id, (Guid)guild.Id);
		}
	}
}
