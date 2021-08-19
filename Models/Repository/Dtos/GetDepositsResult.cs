namespace HuokanServer.Models.Repository.Dtos
{
	public class GetDepositsResult
	{
		public int Id { get; set; }
		public int Endorsements { get; set; }
		public string CharacterName { get; set; }
		public long DepositInCopper { get; set; }
	}
}
