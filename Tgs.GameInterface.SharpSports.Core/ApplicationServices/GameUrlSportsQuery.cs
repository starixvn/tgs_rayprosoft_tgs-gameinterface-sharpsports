namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class GameUrlSportsQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string ClientSecret,
	string PlayerCode,
	bool IsMobile,
	string Language,
	string Ip);
