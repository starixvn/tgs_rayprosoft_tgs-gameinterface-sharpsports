namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class FundResult(
	string OrderId,
	string ExternalOrderId,
	decimal Balance,
	string Resp);
