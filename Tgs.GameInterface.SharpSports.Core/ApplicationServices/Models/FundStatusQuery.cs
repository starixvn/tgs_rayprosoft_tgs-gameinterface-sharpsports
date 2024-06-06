namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;

public sealed record class FundStatusQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string SecretKey,
	string PlayerCode,
	DateTimeOffset OrderOn,
	string OrderId);
