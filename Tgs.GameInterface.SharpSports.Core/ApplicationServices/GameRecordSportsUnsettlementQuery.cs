namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class GameRecordSportsUnsettlementQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string ClientSecret,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime,
	int Page,
	int PageSize);
