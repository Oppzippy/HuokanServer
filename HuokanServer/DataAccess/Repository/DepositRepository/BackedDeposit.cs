namespace HuokanServer.DataAccess.Repository.DepositRepository;

public record BackedDeposit
{
	public int Endorsements { get; init; }
	public string CharacterName { get; init; }
	public string CharacterRealm { get; set; }
	public long DepositInCopper { get; init; }
}