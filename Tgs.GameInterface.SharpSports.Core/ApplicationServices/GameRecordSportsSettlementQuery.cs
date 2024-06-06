namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed record class GameRecordSportsSettlementQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string ClientSecret,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime,
	int Page,
	int PageSize);
