using Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.Core.ApplicationServices;

public sealed class FundStatusGetService(IGameApiClient apiClient) : IGetService<FundStatusQuery, bool>
{
	public async ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
	{
		var response = await apiClient.TransferRecordsAsync(
			apiBaseUrl: query.ApiUrl,
			merchantId: query.MerchantId,
			clientId: query.ClientId,
			clientSecret: query.SecretKey,
			loginName: query.PlayerCode,
			startTime: query.OrderOn.Add(TimeSpan.FromHours(-6)),
			endTime: query.OrderOn.Add(new(hours: 5, minutes: 59, seconds: 59)),
			pageNum: 1,
			pageSize: 200,
			merchantPayNo: query.OrderId,
			cancellationToken)
			.ConfigureAwait(false);

		return response;
	}
}
