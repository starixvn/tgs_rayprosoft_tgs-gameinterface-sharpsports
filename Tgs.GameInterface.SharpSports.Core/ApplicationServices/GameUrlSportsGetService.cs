using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed class GameUrlSportsGetService(IGameApiClient apiClient) : IGetService<GameUrlSportsQuery, GameUrlSportsResult>
{
	public async ValueTask<GameUrlSportsResult> GetAsync(GameUrlSportsQuery query, CancellationToken cancellationToken = default)
	{
		var response = await apiClient.PlayerSportsLoginAsync(
			apiBaseUrl: query.ApiUrl,
			merchantId: query.MerchantId,
			clientId: query.ClientId,
			clientSecret: query.ClientSecret,
			loginName: query.PlayerCode,
			terType: query.IsMobile ? TerType.H5 : TerType.PC,
			language: query.Language,
			ip: query.Ip,
			cancellationToken).
			ConfigureAwait(false);

		return new(
			GameUrl: response);
	}
}
