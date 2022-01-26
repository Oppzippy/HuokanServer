using System;

namespace HuokanServer.DataAccess.Repository.DepositRepository;

public record BackedDeposit
{
	public Guid Id { get; init; }
	public int Endorsements { get; init; }
	public string CharacterName { get; init; }
	public string CharacterRealm { get; init; }
	public long DepositInCopper { get; init; }
	public DateTimeOffset ApproximateDepositTimestamp { get; init; }
}
