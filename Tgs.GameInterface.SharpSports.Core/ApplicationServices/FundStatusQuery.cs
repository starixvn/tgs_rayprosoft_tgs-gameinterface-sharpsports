namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class FundStatusQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string ClientSecret,
	string PlayerCode,
	DateTimeOffset OrderOn,
	string OrderId);
