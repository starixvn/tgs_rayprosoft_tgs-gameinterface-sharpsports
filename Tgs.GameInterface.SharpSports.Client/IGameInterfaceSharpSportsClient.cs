using Tgs.GameInterface.SharpSports.Protos;

namespace Tgs.GameInterface.SharpSports.Client;

public interface IGameInterfaceSharpSportsClient
{
	ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string merchantId, string clientId, string clientSecret, string playerCode, bool isMobile, string language, string ip, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string merchantId, string clientId, string clientSecret, string playerCode, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(Uri apiUrl, string merchantId, string clientId, string clientSecret, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(Uri apiUrl, string merchantId, string clientId, string clientSecret, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(Uri apiUrl, string merchantId, string clientId, string clientSecret, string playerCode, DateTimeOffset orderOn, string orderId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordInfo> GameRecordSettlementAsync(Uri apiUrl, string merchantId, string clientId, string clientSecret, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 500, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordInfo> GameRecordUnsettlementAsync(Uri apiUrl, string merchantId, string clientId, string clientSecret, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 500, CancellationToken cancellationToken = default);
}
