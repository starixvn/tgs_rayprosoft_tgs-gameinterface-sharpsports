namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class GameRecordSportsGameBetSportsItem(
	string ExternalBetId,
	string PlayerCode,
	bool IsClosed,
	bool IsCancel,
	bool IsRefund,
	bool IsVoid,
	bool IsCashout,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	DateTimeOffset ExternalPayoffOn,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal WinLoss,
	string AdditionalData);
