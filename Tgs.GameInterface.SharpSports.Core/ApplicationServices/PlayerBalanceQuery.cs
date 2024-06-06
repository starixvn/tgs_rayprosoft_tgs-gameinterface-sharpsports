namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class PlayerBalanceQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string ClientSecret,
	string PlayerCode);
