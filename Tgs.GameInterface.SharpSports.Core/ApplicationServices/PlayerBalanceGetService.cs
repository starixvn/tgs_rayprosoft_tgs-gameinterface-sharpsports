using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed class PlayerBalanceGetService(IGameApiClient apiClient) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var response = await apiClient.PlayerBalanceAsync(
			apiBaseUrl: query.ApiUrl,
			merchantId: query.MerchantId,
			clientId: query.ClientId,
			clientSecret: query.ClientSecret,
			loginName: query.PlayerCode,
			cancellationToken)
			.ConfigureAwait(false);

		return new(
			IsPlayerExist: response.IsPlayerExist,
			Balance: response.Balance);
	}
}
