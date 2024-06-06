using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed class FundInGetService(IGameApiClient apiClient) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var response = await apiClient.WalletDepositAndWithDrawAsync(
			apiBaseUrl: query.ApiUrl,
			merchantId: query.MerchantId,
			clientId: query.ClientId,
			clientSecret: query.ClientSecret,
			loginName: query.PlayerCode,
			merchantPayNo: query.OrderId,
			tradeType: TradeType.WithDraw,
			amount: query.Amount,
			cancellationToken)
			.ConfigureAwait(false);

		return new(
			OrderId: response.MerchantPayNo,
			ExternalOrderId: response.TradeNo,
			response.Balance,
			response.Resp);
	}
}
