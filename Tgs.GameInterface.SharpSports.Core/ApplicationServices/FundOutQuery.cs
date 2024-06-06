namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class FundOutQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string ClientSecret,
	string PlayerCode,
	string OrderId,
	decimal Amount);
