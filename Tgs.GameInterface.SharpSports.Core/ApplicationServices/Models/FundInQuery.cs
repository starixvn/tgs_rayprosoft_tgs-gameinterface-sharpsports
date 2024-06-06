namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;

public sealed record class FundInQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string SecretKey,
	string PlayerCode,
	string OrderId,
	decimal Amount);
