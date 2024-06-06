using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed class GameRecordSportsSettlement(IGameApiClient apiClient) : IQueryService<GameRecordSportsSettlementQuery, GameRecordSportsGameBetSportsItem>
{
	public async IAsyncEnumerable<GameRecordSportsGameBetSportsItem> QueryAsync(GameRecordSportsSettlementQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var asyncEnumerable = apiClient.SportOrderListAsync(
			apiBaseUrl: query.ApiUrl,
			merchantId: query.MerchantId,
			clientId: query.ClientId,
			clientSecret: query.ClientSecret,
			beginTime: query.StartTime,
			endTime: query.EndTime,
			timeType: SportOrderListTimeType.ResultDate,
			pageNum: query.Page,
			pageSize: query.PageSize,
			cancellationToken);

		await foreach (var item in asyncEnumerable)
			yield return new(
				ExternalBetId: item.OrderId,
				PlayerCode: item.LoginName,
				IsClosed: item.State is not (0 or 1),
				IsCancel: item.State is 5 or 7,
				IsRefund: item.State is 6,
				IsVoid: item.State is 8,
				IsCashout: item.CashoutType is not "0",
				ExternalCreatedOn: item.CreateDate,
				ExternalClosedOn: item.ResultDate,
				ExternalPayoffOn: item.RealResultDate,
				Bet: item.Gold,
				ValidBet: item.RealGold,
				Settle: item.RealGold + item.WinGold,
				WinLoss: item.WinGold,
				AdditionalData: item.AdditionalData);
	}
}
