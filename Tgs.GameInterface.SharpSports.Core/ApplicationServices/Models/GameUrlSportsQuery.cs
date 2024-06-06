namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;

public sealed record class GameUrlSportsQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string SecretKey,
	string PlayerCode,
	bool IsMobile,
	string Language,
	string Ip);
