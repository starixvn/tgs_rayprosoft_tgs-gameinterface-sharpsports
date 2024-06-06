using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.SharpSports.Protos;

namespace Tgs.GameInterface.SharpSports.Client;

internal sealed class GameInterfaceSharpSportsClient(
	GameInterfaceSharpSports.GameInterfaceSharpSportsClient client)
	: IGameInterfaceSharpSportsClient
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client.CheckingServiceAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string merchantId, string clientId, string secretKey, string playerCode, bool isMobile, string language, string ip, CancellationToken cancellationToken = default)
	{
		var request = new GameUrlRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			ClientId = clientId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			IsMobile = isMobile,
			Language = language,
			Ip = ip,
		};

		return await client.GameUrlAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string merchantId, string clientId, string secretKey, string playerCode, CancellationToken cancellationToken = default)
	{
		var request = new PlayerBalanceRequest()
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			ClientId = clientId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
		};

		return await client.PlayerBalanceAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string merchantId, string clientId, string secretKey, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			ClientId = clientId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			OrderId = orderId,
			Amount = amount,
		};
		return await client.FundInAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string merchantId, string clientId, string secretKey, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			ClientId = clientId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			OrderId = orderId,
			Amount = amount,
		};
		return await client.FundOutAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string merchantId, string clientId, string secretKey, string playerCode, DateTimeOffset orderOn, string orderId, CancellationToken cancellationToken = default)
	{
		var request = new FundStatusRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			ClientId = clientId,
			SecretKey = secretKey,
			PlayerCode = playerCode,
			OrderOn = orderOn.ToTimestamp(),
			OrderId = orderId,
		};
		var reply = await client.FundStatusAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.Status;
	}

	public async IAsyncEnumerable<GameRecordInfo> GameRecordSettlementAsync(Uri apiUrl, string merchantId, string clientId, string secretKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 500, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new GameRecordRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			ClientId = clientId,
			SecretKey = secretKey,
			StartTime = startTime.ToTimestamp(),
			EndTime = endTime.ToTimestamp(),
			Page = page,
			PageSize = pageSize,
		};

		var reply = await client.GameRecordSettlementAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		foreach (var info in reply.Items)
			yield return info;
	}

	public async IAsyncEnumerable<GameRecordInfo> GameRecordUnsettlementAsync(Uri apiUrl, string merchantId, string clientId, string secretKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 500, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new GameRecordRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			ClientId = clientId,
			SecretKey = secretKey,
			StartTime = startTime.ToTimestamp(),
			EndTime = endTime.ToTimestamp(),
			Page = page,
			PageSize = pageSize,
		};

		var reply = await client.GameRecordUnsettlementAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		foreach (var info in reply.Items)
			yield return info;
	}
}
