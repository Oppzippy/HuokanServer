namespace HuokanServer.Models.Repository.DepositRepository
{
	public record GetDepositsResult
	{
		public int Id { get; init; }
		public int Endorsements { get; init; }
		public string CharacterName { get; init; }
		public long DepositInCopper { get; init; }
	}
}
