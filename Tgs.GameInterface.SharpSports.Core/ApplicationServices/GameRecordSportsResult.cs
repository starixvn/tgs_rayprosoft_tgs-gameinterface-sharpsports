namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class GameRecordSportsResult(
	string ExternalBetId,
	string PlayerCode,
	SportOrderListState State,
	bool IsCashout,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	DateTimeOffset ExternalPayoffOn,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal WinLoss,
	string AdditionalData);
