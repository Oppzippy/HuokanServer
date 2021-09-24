namespace HuokanServer.DataAccess.Repository.DepositRepository
{
	public record BackedDeposit
	{
		public int Endorsements { get; init; }
		public string CharacterName { get; init; }
		public long DepositInCopper { get; init; }
	}
}
