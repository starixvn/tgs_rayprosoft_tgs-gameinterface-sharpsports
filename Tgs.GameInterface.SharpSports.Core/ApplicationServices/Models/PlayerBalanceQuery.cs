namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;

public sealed record class PlayerBalanceQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string SecretKey,
	string PlayerCode);
