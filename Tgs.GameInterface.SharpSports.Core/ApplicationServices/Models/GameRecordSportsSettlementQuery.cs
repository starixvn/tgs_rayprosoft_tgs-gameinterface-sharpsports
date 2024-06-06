namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;

public sealed record class GameRecordSportsSettlementQuery(
	Uri ApiUrl,
	string MerchantId,
	string ClientId,
	string SecretKey,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime,
	int Page,
	int PageSize);
