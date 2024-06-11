using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed class GameRecordSportsSettlement(IGameApiClient apiClient) : IQueryService<GameRecordSportsSettlementQuery, GameRecordSportsResult>
{
	public async IAsyncEnumerable<GameRecordSportsResult> QueryAsync(GameRecordSportsSettlementQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var asyncEnumerable = apiClient.SportOrderListAsync(
			apiBaseUrl: query.ApiUrl,
			merchantId: query.MerchantId,
			clientId: query.ClientId,
			clientSecret: query.ClientSecret,
			beginTime: query.StartTime,
			endTime: query.EndTime,
			orderState: SportOrderListOrderState.Settled,
			timeType: SportOrderListTimeType.ResultDate,
			pageNum: query.Page,
			pageSize: query.PageSize,
			cancellationToken);

		await foreach (var item in asyncEnumerable)
		{
			var isCashout = item.CashoutType is not "0";

			yield return new(
				ExternalBetId: item.OrderId,
				PlayerCode: item.LoginName,
				State: item.State,
				IsCashout: isCashout,
				ExternalCreatedOn: item.CreateDate,
				ExternalClosedOn: item.ResultDate,
				ExternalPayoffOn: item.RealResultDate,
				Bet: item.Gold,
				ValidBet: item.RealGold,
				Settle: item.WinGold,
				WinLoss: item.WinGold - item.Gold,
				AdditionalData: item.AdditionalData);
		}
	}
}
