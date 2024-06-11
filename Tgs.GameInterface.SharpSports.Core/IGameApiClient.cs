using Tgs.GameInterface.SharpSports.Core.Models;

namespace Tgs.GameInterface.SharpSports.Core;

public interface IGameApiClient
{
	ValueTask<string> PlayerSportsLoginAsync(Uri apiBaseUrl, string merchantId, string clientId, string clientSecret, string loginName, TerType terType, string language, string ip, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceResponse> PlayerBalanceAsync(Uri apiBaseUrl, string merchantId, string clientId, string clientSecret, string loginName, CancellationToken cancellationToken = default);

	ValueTask<WalletDepositAndWithDrawResponse> WalletDepositAndWithDrawAsync(Uri apiBaseUrl, string merchantId, string clientId, string clientSecret, string loginName, string merchantPayNo, TradeType tradeType, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<bool> TransferRecordsAsync(Uri apiBaseUrl, string merchantId, string clientId, string clientSecret, string loginName, DateTimeOffset startTime, DateTimeOffset endTime, int pageNum, int pageSize, string merchantPayNo, CancellationToken cancellationToken = default);

	IAsyncEnumerable<SportOrderListResponseItem> SportOrderListAsync(Uri apiBaseUrl, string merchantId, string clientId, string clientSecret, DateTimeOffset beginTime, DateTimeOffset endTime, SportOrderListOrderState orderState, SportOrderListTimeType timeType, int pageNum, int pageSize, CancellationToken cancellationToken = default);
}
