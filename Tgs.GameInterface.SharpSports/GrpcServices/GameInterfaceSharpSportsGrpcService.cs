using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;
using Tgs.GameInterface.SharpSports.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SharpSports.GrpcServices;

internal sealed class GameInterfaceSharpSportsGrpcService(
	ILogger<GameInterfaceSharpSportsGrpcService> logger,
	IGetService<FundInQuery, FundResult> fundInService,
	IGetService<FundOutQuery, FundResult> fundOutService,
	IGetService<FundStatusQuery, bool> fundStatusService,
	IGetService<GameUrlSportsQuery, GameUrlSportsResult> gameUrlService,
	IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
	IQueryService<GameRecordSportsSettlementQuery, GameRecordSportsGameBetSportsItem> gameRecordSportsSettlementService,
	IQueryService<GameRecordSportsUnsettlementQuery, GameRecordSportsGameBetSportsItem> gameRecordSportsUnsettlementService)
	: GameInterfaceSharpSports.GameInterfaceSharpSportsBase
{
	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName,
		});
	}

	public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var query = new GameUrlSportsQuery(
			ApiUrl: new Uri(request.ApiUrl),
			MerchantId: request.MerchantId,
			ClientId: request.ClientId,
			ClientSecret: request.ClientSecret,
			PlayerCode: request.PlayerCode,
			IsMobile: request.IsMobile,
			Language: request.Language,
			Ip: request.Ip);

		var result = await gameUrlService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new()
		{
			GameUrl = result.GameUrl,
		};
	}

	public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri(request.ApiUrl),
			MerchantId: request.MerchantId,
			ClientId: request.ClientId,
			ClientSecret: request.ClientSecret,
			PlayerCode: request.PlayerCode);

		var result = await playerBalanceService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new()
		{
			IsPlayerExist = result.IsPlayerExists,
			Balance = result.Balance
		};
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var query = new FundInQuery(
			ApiUrl: new Uri(request.ApiUrl),
			MerchantId: request.MerchantId,
			ClientId: request.ClientId,
			ClientSecret: request.ClientSecret,
			PlayerCode: request.PlayerCode,
			OrderId: request.OrderId,
			Amount: request.Amount);

		var result = await fundInService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new()
		{
			Balance = result.Balance,
			Resp = result.Resp,
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var query = new FundOutQuery(
			ApiUrl: new Uri(request.ApiUrl),
			MerchantId: request.MerchantId,
			ClientId: request.ClientId,
			ClientSecret: request.ClientSecret,
			PlayerCode: request.PlayerCode,
			OrderId: request.OrderId,
			Amount: request.Amount);

		var result = await fundOutService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new()
		{
			Balance = result.Balance,
			Resp = result.Resp,
		};
	}

	public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
	{
		var query = new FundStatusQuery(
			ApiUrl: new Uri(request.ApiUrl),
			MerchantId: request.MerchantId,
			ClientId: request.ClientId,
			ClientSecret: request.ClientSecret,
			PlayerCode: request.PlayerCode,
			OrderOn: request.OrderOn.ToDateTimeOffset(),
			OrderId: request.OrderId);

		var result = await fundStatusService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new()
		{
			Status = result,
		};
	}

	public override async Task<GameRecordReply> GameRecordSettlement(GameRecordRequest request, ServerCallContext context)
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new Uri(request.ApiUrl),
			MerchantId: request.MerchantId,
			ClientId: request.ClientId,
			ClientSecret: request.ClientSecret,
			StartTime: request.StartTime.ToDateTimeOffset(),
			EndTime: request.EndTime.ToDateTimeOffset(),
			request.Page,
			request.PageSize);

		var results = await gameRecordSportsSettlementService
			.QueryAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ToArrayAsync()
			.ConfigureAwait(false);

		var reply = new GameRecordReply();

		if (results.Length != 0)
		{
			foreach (var result in results)
			{
				reply.Items.Add(new GameRecordInfo
				{
					ExternalBetId = result.ExternalBetId,
					PlayerCode = result.PlayerCode,
					IsClosed = result.IsClosed,
					IsCancel = result.IsCancel,
					IsRefund = result.IsRefund,
					IsVoid = result.IsVoid,
					IsCashout = result.IsCashout,
					ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
					ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
					ExternalPayoffOn = result.ExternalPayoffOn.ToTimestamp(),
					Bet = result.Bet,
					ValidBet = result.ValidBet,
					Settle = result.Settle,
					WinLoss = result.WinLoss,
					AdditionalData = result.AdditionalData
				});
			}
		}

		return reply;
	}

	public override async Task<GameRecordReply> GameRecordUnsettlement(GameRecordRequest request, ServerCallContext context)
	{
		var query = new GameRecordSportsUnsettlementQuery(
			ApiUrl: new Uri(request.ApiUrl),
			MerchantId: request.MerchantId,
			ClientId: request.ClientId,
			ClientSecret: request.ClientSecret,
			StartTime: request.StartTime.ToDateTimeOffset(),
			EndTime: request.EndTime.ToDateTimeOffset(),
			request.Page,
			request.PageSize);

		var results = await gameRecordSportsUnsettlementService
			.QueryAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ToArrayAsync()
			.ConfigureAwait(false);

		var reply = new GameRecordReply();

		if (results.Length != 0)
		{
			foreach (var result in results)
			{
				reply.Items.Add(new GameRecordInfo
				{
					ExternalBetId = result.ExternalBetId,
					PlayerCode = result.PlayerCode,
					IsClosed = result.IsClosed,
					IsCancel = result.IsCancel,
					IsRefund = result.IsRefund,
					IsVoid = result.IsVoid,
					IsCashout = result.IsCashout,
					ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
					ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
					ExternalPayoffOn = result.ExternalPayoffOn.ToTimestamp(),
					Bet = result.Bet,
					ValidBet = result.ValidBet,
					Settle = result.Settle,
					WinLoss = result.WinLoss,
					AdditionalData = result.AdditionalData
				});
			}
		}

		return reply;
	}
}
