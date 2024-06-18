namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class PlayerBalanceResult(
	bool IsPlayerExist,
	decimal Balance);
