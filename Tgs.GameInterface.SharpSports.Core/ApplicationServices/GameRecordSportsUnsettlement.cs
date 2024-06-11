using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed class GameRecordSportsUnsettlement(IGameApiClient apiClient) : IQueryService<GameRecordSportsUnsettlementQuery, GameRecordSportsGameBetSportsItem>
{
	public async IAsyncEnumerable<GameRecordSportsGameBetSportsItem> QueryAsync(GameRecordSportsUnsettlementQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var asyncEnumerable = apiClient.SportOrderListAsync(
			apiBaseUrl: query.ApiUrl,
			merchantId: query.MerchantId,
			clientId: query.ClientId,
			clientSecret: query.ClientSecret,
			beginTime: query.StartTime,
			endTime: query.EndTime,
			orderState: SportOrderListOrderState.Unsettled,
			timeType: SportOrderListTimeType.CreateDate,
			pageNum: query.Page,
			pageSize: query.PageSize,
			cancellationToken);

		await foreach (var item in asyncEnumerable)
			yield return new(
				ExternalBetId: item.OrderId,
				PlayerCode: item.LoginName,
				IsClosed: false,
				IsCancel: false,
				IsRefund: false,
				IsVoid: false,
				IsCashout: false,
				ExternalCreatedOn: item.CreateDate,
				ExternalClosedOn: DateTimeOffset.UnixEpoch,
				ExternalPayoffOn: DateTimeOffset.UnixEpoch,
				Bet: item.Gold,
				ValidBet: item.RealGold,
				Settle: 0.0m,
				WinLoss: 0.0m,
				AdditionalData: item.AdditionalData);
	}
}
