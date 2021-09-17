using System.Collections.Generic;
using System.Threading.Tasks;
using HuokanServer.Models.Repository.DepositRepository;
using HuokanServer.Models.Repository.GuildRepository;
using HuokanServer.Models.Repository.UserRepository;
using Xunit;

namespace HuokanServer.IntegrationTests.Models.Repository.DepositRepositoryTest
{
	public class ImportTest : DepositRepositoryTestBase
	{
		[Fact]
		public async Task TestImport()
		{
			BackedGuild guild = await CreateGuild();
			BackedUser user = await CreateUser(guild.OrganizationId);
			await Repository.Import(guild.OrganizationId, guild.Id, user.Id, new List<Deposit>()
			{
				new Deposit(){
					CharacterName = "Advertiser-Illidan",
					DepositInCopper = 1,
					GuildBankCopper = 1,
				},
				new Deposit(){
					CharacterName = "Advertiser2-Illidan",
					DepositInCopper = 2,
					GuildBankCopper = 3,
				},
			});
		}
	}
}
