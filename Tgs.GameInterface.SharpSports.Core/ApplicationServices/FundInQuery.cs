namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class FundInQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string ClientSecret,
	string PlayerCode,
	string OrderId,
	decimal Amount);
