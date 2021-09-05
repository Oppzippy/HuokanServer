using System;
using HuokanServer.Models.Repository.DepositRepository;
using Xunit;

namespace HuokanServer.IntegrationTests
{
	public class Test1 : IClassFixture<DatabaseFixture>
	{
		private readonly DatabaseFixture _dbFixture;

		public Test1(DatabaseFixture dbFixture)
		{
			this._dbFixture = dbFixture;
		}

		// [Fact]
		// public async void TestCreateDeposit()
		// {
		// 	var repo = new DepositRepository(_dbFixture.DbConnection);
		// 	var result = await repo.CreateDeposit(new CreateDepositsArgs
		// 	{
		// 		GraphId = 1,
		// 		CharacterName = "Testname-Testrealm",
		// 		DepositInCopper = 100,
		// 		GuildBankCopper = 100,
		// 		CreatedAt = DateTime.UtcNow,
		// 	});
		// 	Assert.NotNull(result);
		// }
	}
}
