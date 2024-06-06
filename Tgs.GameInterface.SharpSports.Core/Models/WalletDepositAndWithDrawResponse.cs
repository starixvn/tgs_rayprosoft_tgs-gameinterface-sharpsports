namespace Tgs.GameInterface.SharpSports.Core.Models;

public sealed record class WalletDepositAndWithDrawResponse(
	string TradeNo,
	string MerchantPayNo,
	decimal Balance,
	string Resp);
