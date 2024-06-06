using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.Controllers;

[Route("[controller]/[action]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public sealed class SharpSportsController : ControllerBase
{
	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "https://api.98ktest.com",
	///     "merchantId": "c6a33e4e235e731b51d2f303199edeb4",
	///     "clientId": "2a954fedf9a203d646e1a7f3c32528df",
	///     "ClientSecret": "1ff45c6ed7494b04940dc0bd9e0dca6b",
	///     "playerCode": "4ucmqplxb029",
	///     "isMobile": true,
	///     "language": "zh-cn",
	///     "ip": "127.0.0.1"
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<GameUrlSportsResult> GameUrlSports(
		[FromServices] IGetService<GameUrlSportsQuery, GameUrlSportsResult> service,
		[FromBody] GameUrlSportsQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "https://api.yuansports1.com/",
	///     "merchantId": "c6a33e4e235e731b51d2f303199edeb4",
	///     "clientId": "2a954fedf9a203d646e1a7f3c32528df",
	///     "ClientSecret": "1ff45c6ed7494b04940dc0bd9e0dca6b",
	///     "playerCode": "4ucmqplxb029"
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<PlayerBalanceResult> PlayerBalance(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		[FromBody] PlayerBalanceQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "https://api.yuansports1.com/",
	///     "merchantId": "c6a33e4e235e731b51d2f303199edeb4",
	///     "clientId": "2a954fedf9a203d646e1a7f3c32528df",
	///     "ClientSecret": "1ff45c6ed7494b04940dc0bd9e0dca6b",
	///     "playerCode": "4ucmqplxb029",
	///     "orderId": "XXXXX",
	///     "amount": 10
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<FundResult> FundIn(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		[FromBody] FundInQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "https://api.yuansports1.com/",
	///     "merchantId": "c6a33e4e235e731b51d2f303199edeb4",
	///     "clientId": "2a954fedf9a203d646e1a7f3c32528df",
	///     "ClientSecret": "1ff45c6ed7494b04940dc0bd9e0dca6b",
	///     "playerCode": "4ucmqplxb029",
	///     "orderId": "XXXXX",
	///     "amount": 10
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<FundResult> FundOut(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		[FromBody] FundOutQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "https://api.yuansports1.com/",
	///     "merchantId": "c6a33e4e235e731b51d2f303199edeb4",
	///     "clientId": "2a954fedf9a203d646e1a7f3c32528df",
	///     "ClientSecret": "1ff45c6ed7494b04940dc0bd9e0dca6b",
	///     "playerCode": "4ucmqplxb029",
	///     "orderOn": "2024-06-03T17:00:00+08:00",
	///     "orderId": "XXXXX"
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<bool> FundStatus(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		[FromBody] FundStatusQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "https://api.yuansports1.com/",
	///     "merchantId": "c6a33e4e235e731b51d2f303199edeb4",
	///     "clientId": "2a954fedf9a203d646e1a7f3c32528df",
	///     "ClientSecret": "1ff45c6ed7494b04940dc0bd9e0dca6b",
	///     "playerCode": "4ucmqplxb029",
	///     "startTime": "2024-06-03T17:00:00+08:00",
	///     "endTime": "2024-06-03T17:10:00+08:00",
	///     "page": 1,
	///     "pageSize": 500
	/// }
	/// </remarks>
	[HttpPost]
	public async IAsyncEnumerable<GameRecordSportsGameBetSportsItem> GameRecordSportsSettlement(
		[FromServices] IQueryService<GameRecordSportsSettlementQuery, GameRecordSportsGameBetSportsItem> service,
		[FromBody] GameRecordSportsSettlementQuery query)
	{
		var items = await service.QueryAsync(query).ToArrayAsync().ConfigureAwait(false);
		foreach (var item in items)
			yield return item;
	}

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "https://api.yuansports1.com/",
	///     "merchantId": "c6a33e4e235e731b51d2f303199edeb4",
	///     "clientId": "2a954fedf9a203d646e1a7f3c32528df",
	///     "ClientSecret": "1ff45c6ed7494b04940dc0bd9e0dca6b",
	///     "playerCode": "4ucmqplxb029",
	///     "startTime": "2024-06-03T17:00:00+08:00",
	///     "endTime": "2024-06-03T17:10:00+08:00",
	///     "page": 1,
	///     "pageSize": 500
	/// }
	/// </remarks>
	[HttpPost]
	public async IAsyncEnumerable<GameRecordSportsGameBetSportsItem> GameRecordSportsUnsettlement(
		[FromServices] IQueryService<GameRecordSportsUnsettlementQuery, GameRecordSportsGameBetSportsItem> service,
		[FromBody] GameRecordSportsUnsettlementQuery query)
	{
		var items = await service.QueryAsync(query).ToArrayAsync().ConfigureAwait(false);
		foreach (var item in items)
			yield return item;
	}
}
