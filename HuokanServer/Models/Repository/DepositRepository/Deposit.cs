namespace HuokanServer.Models.Repository.DepositRepository
{
	public record Deposit
	{
		public string CharacterName { get; init; }
		public long DepositInCopper { get; init; }
		public long GuildBankCopper { get; init; }
	}
}
