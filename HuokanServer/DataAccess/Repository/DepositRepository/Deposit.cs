using System;

namespace HuokanServer.DataAccess.Repository.DepositRepository;

public record Deposit
{
	public string CharacterName { get; init; }
	public string CharacterRealm { get; init; }
	public long DepositInCopper { get; init; }
	public long GuildBankCopper { get; init; }
	public DateTimeOffset ApproximateDepositTimestamp { get; init; }
}
