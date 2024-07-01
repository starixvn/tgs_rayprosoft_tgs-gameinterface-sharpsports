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

			/*
			 * 赢或者输的情况
			 * 1、单线且非提前结算---》 有效投注额取本金
			 * 2、双线且非提前结算---》 有效投注额取盈亏金额的绝对值
			 * 3、提前结算----》有效投注额取盈亏的绝对值
			 * 4、所有串关都算单线
			 */

			var bet = item.Gold;
			var validBet = item.RealGold;

			if (item.LineType == "1" && item.CashoutType == "0")
			{
				validBet = bet;
			}
			else
			{
				if (bet < validBet)
				{
					validBet = bet;
				}
			}

			yield return new(
				ExternalBetId: item.OrderId,
				PlayerCode: item.LoginName,
				State: item.State,
				IsCashout: isCashout,
				ExternalCreatedOn: item.CreateDate,
				ExternalClosedOn: item.ResultDate,
				ExternalPayoffOn: item.RealResultDate,
				Bet: bet,
				ValidBet: validBet,
				Settle: item.WinGold,
				WinLoss: item.WinGold - item.Gold,
				AdditionalData: item.AdditionalData);
		}
	}
}
