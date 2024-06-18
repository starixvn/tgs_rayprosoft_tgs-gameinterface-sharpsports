using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.SharpSports.Client;
using Tgs.GameInterface.SharpSports.Core;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;
using Tgs.GameInterface.SharpSports.Protos;
using Tgs.ServiceUtility;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tgs.GameInterface.SharpSports.IntegrationTest;

public sealed class GameInterfaceSharpSportsGrpcServiceTests
{
	private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

	[Fact]
	public async Task CheckingService()
	{
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.AddSingleton(Substitute.For<IGameApiClient>());
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceSharpSports.GameInterfaceSharpSportsClient(channel);
		var sut = new GameInterfaceSharpSportsClient(client);

		var actual = await sut.CheckingServiceAsync(Environment.MachineName).ConfigureAwait(true);

		Assert.Equal(Environment.MachineName, actual);
	}

	[Fact]
	public async Task GameUrl()
	{
		var grpcRequest = new GameUrlRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
			IsMobile = true,
			Language = "zh-cn",
			Ip = "127.0.0.1",
		};
		var serviceRequest = new GameUrlSportsQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			ClientSecret: grpcRequest.ClientSecret,
			PlayerCode: grpcRequest.PlayerCode,
			IsMobile: grpcRequest.IsMobile,
			Language: grpcRequest.Language,
			Ip: grpcRequest.Ip);
		var serviceResponse = new GameUrlSportsResult(
			GameUrl: "https://xxx.xxx.xxx/xxx");
		var fakeService = Substitute.For<IGetService<GameUrlSportsQuery, GameUrlSportsResult>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<GameUrlSportsQuery, GameUrlSportsResult>)));
				services.AddSingleton(Substitute.For<IGameApiClient>());
				services.AddSingleton(fakeService);
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceSharpSports.GameInterfaceSharpSportsClient(channel);
		var sut = new GameInterfaceSharpSportsClient(client);

		var actual = await sut.GameUrlAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			merchantId: grpcRequest.MerchantId,
			clientId: grpcRequest.ClientId,
			clientSecret: grpcRequest.ClientSecret,
			playerCode: grpcRequest.PlayerCode,
			isMobile: grpcRequest.IsMobile,
			language: grpcRequest.Language,
			ip: grpcRequest.Ip)
			.ConfigureAwait(true);

		Assert.Equal("https://xxx.xxx.xxx/xxx", actual.GameUrl);
	}

	[Fact]
	public async Task PlayerBalance()
	{
		var grpcRequest = new PlayerBalanceRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
		};
		var serviceRequest = new PlayerBalanceQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			ClientSecret: grpcRequest.ClientSecret,
			PlayerCode: grpcRequest.PlayerCode);
		var serviceResponse = new PlayerBalanceResult(
			IsPlayerExist: true,
			Balance: 10m);
		var fakeService = Substitute.For<IGetService<PlayerBalanceQuery, PlayerBalanceResult>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<PlayerBalanceQuery, PlayerBalanceResult>)));
				services.AddSingleton(Substitute.For<IGameApiClient>());
				services.AddSingleton(fakeService);
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceSharpSports.GameInterfaceSharpSportsClient(channel);
		var sut = new GameInterfaceSharpSportsClient(client);

		var actual = await sut.PlayerBalanceAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			merchantId: grpcRequest.MerchantId,
			clientId: grpcRequest.ClientId,
			clientSecret: grpcRequest.ClientSecret,
			playerCode: grpcRequest.PlayerCode)
			.ConfigureAwait(true);

		Assert.True(actual.IsPlayerExist);
		Assert.Equal(10m, (decimal)actual.Balance);
	}

	[Fact]
	public async Task FundIn()
	{
		var grpcRequest = new FundRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
			OrderId = "01HZ1GGQFKP2278WJVBVGYT7NX",
			Amount = 6m,
		};
		var serviceRequest = new FundInQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			ClientSecret: grpcRequest.ClientSecret,
			PlayerCode: grpcRequest.PlayerCode,
			OrderId: grpcRequest.OrderId,
			Amount: grpcRequest.Amount);
		var json = new JsonObject
		{
			["tradeNo"] = "01HZ1G8FW3KM1W10NZFMNCVE6C",
			["merchantPayNo"] = grpcRequest.OrderId,
			["balance"] = 10m,
		};
		var serviceResponse = new FundResult(
			ExternalOrderId: json["tradeNo"]!.GetValue<string>(),
			OrderId: json["merchantPayNo"]!.GetValue<string>(),
			Balance: json["balance"]!.GetValue<decimal>(),
			Resp: json.ToJsonString());
		var fakeService = Substitute.For<IGetService<FundInQuery, FundResult>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<FundInQuery, FundResult>)));
				services.AddSingleton(Substitute.For<IGameApiClient>());
				services.AddSingleton(fakeService);
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceSharpSports.GameInterfaceSharpSportsClient(channel);
		var sut = new GameInterfaceSharpSportsClient(client);

		var actual = await sut.FundInAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			merchantId: grpcRequest.MerchantId,
			clientId: grpcRequest.ClientId,
			clientSecret: grpcRequest.ClientSecret,
			playerCode: grpcRequest.PlayerCode,
			orderId: grpcRequest.OrderId,
			amount: grpcRequest.Amount)
			.ConfigureAwait(true);

		Assert.Equal(10m, (decimal)actual.Balance);
		Assert.Equal(json.ToJsonString(), actual.Resp);
	}

	[Fact]
	public async Task FundOut()
	{
		var grpcRequest = new FundRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
			OrderId = "01HZ1GGQFKP2278WJVBVGYT7NX",
			Amount = 6m,
		};
		var serviceRequest = new FundOutQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			ClientSecret: grpcRequest.ClientSecret,
			PlayerCode: grpcRequest.PlayerCode,
			OrderId: grpcRequest.OrderId,
			Amount: grpcRequest.Amount);
		var json = new JsonObject
		{
			["tradeNo"] = "01HZ1G8FW3KM1W10NZFMNCVE6C",
			["merchantPayNo"] = grpcRequest.OrderId,
			["balance"] = 10m,
		};
		var serviceResponse = new FundResult(
			ExternalOrderId: json["tradeNo"]!.GetValue<string>(),
			OrderId: json["merchantPayNo"]!.GetValue<string>(),
			Balance: json["balance"]!.GetValue<decimal>(),
			Resp: json.ToJsonString());
		var fakeService = Substitute.For<IGetService<FundOutQuery, FundResult>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<FundOutQuery, FundResult>)));
				services.AddSingleton(Substitute.For<IGameApiClient>());
				services.AddSingleton(fakeService);
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceSharpSports.GameInterfaceSharpSportsClient(channel);
		var sut = new GameInterfaceSharpSportsClient(client);

		var actual = await sut.FundOutAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			merchantId: grpcRequest.MerchantId,
			clientId: grpcRequest.ClientId,
			clientSecret: grpcRequest.ClientSecret,
			playerCode: grpcRequest.PlayerCode,
			orderId: grpcRequest.OrderId,
			amount: grpcRequest.Amount)
			.ConfigureAwait(true);

		Assert.Equal(10m, (decimal)actual.Balance);
		Assert.Equal(json.ToJsonString(), actual.Resp);
	}

	[Fact]
	public async Task FundStatus()
	{
		var grpcRequest = new FundStatusRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
			OrderOn = new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			OrderId = "01HZ1GM13J5QZGXGH4JW7AJFXT",
		};
		var serviceRequest = new FundStatusQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			ClientSecret: grpcRequest.ClientSecret,
			PlayerCode: grpcRequest.PlayerCode,
			OrderOn: grpcRequest.OrderOn.ToDateTimeOffset(),
			OrderId: grpcRequest.OrderId);
		var serviceResponse = true;
		var fakeService = Substitute.For<IGetService<FundStatusQuery, bool>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<FundStatusQuery, bool>)));
				services.AddSingleton(Substitute.For<IGameApiClient>());
				services.AddSingleton(fakeService);
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceSharpSports.GameInterfaceSharpSportsClient(channel);
		var sut = new GameInterfaceSharpSportsClient(client);

		var actual = await sut.FundStatusAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			merchantId: grpcRequest.MerchantId,
			clientId: grpcRequest.ClientId,
			clientSecret: grpcRequest.ClientSecret,
			playerCode: grpcRequest.PlayerCode,
			orderOn: grpcRequest.OrderOn.ToDateTimeOffset(),
			orderId: grpcRequest.OrderId)
			.ConfigureAwait(true);

		Assert.True(actual);
	}

	[Fact]
	public async Task GameRecordSportsSettlement()
	{
		var grpcRequest = new GameRecordRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime = new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			EndTime = new DateTimeOffset(2024, 05, 29, 13, 00, 00, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			Page = 1,
			PageSize = 200,
		};
		var serviceRequest = new GameRecordSportsSettlementQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			ClientSecret: grpcRequest.ClientSecret,
			StartTime: grpcRequest.StartTime.ToDateTimeOffset(),
			EndTime: grpcRequest.EndTime.ToDateTimeOffset(),
			Page: grpcRequest.Page,
			PageSize: grpcRequest.PageSize);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ1F630052YMYPME9ZRVWKM7",
			["state"] = 2,
			["createDate"] = "2024-05-29 01:00:00",
			["resultDate"] = "2024-05-29 01:00:10",
			["realResultDate"] = "2024-05-29 01:00:12",
			["gold"] = 10m,
			["realGold"] = 10m,
			["winGold"] = 2m,
			["cashoutType"] = "0",
		};
		var serviceResponseItem = new GameRecordSportsResult(
			ExternalBetId: json["orderId"]!.GetValue<string>(),
			PlayerCode: json["loginName"]!.GetValue<string>(),
			State: SportOrderListState.Win,
			IsCashout: json["cashoutType"]!.GetValue<string>() is not "0",
			ExternalCreatedOn: (DateTime.Parse(json["createDate"]!.GetValue<string>()) is DateTime dt1) ? new DateTimeOffset(dt1, _TimeZone.GetUtcOffset(dt1)) : throw new(),
			ExternalClosedOn: (DateTime.Parse(json["resultDate"]!.GetValue<string>()) is DateTime dt2) ? new DateTimeOffset(dt2, _TimeZone.GetUtcOffset(dt2)) : throw new(),
			ExternalPayoffOn: (DateTime.Parse(json["realResultDate"]!.GetValue<string>()) is DateTime dt3) ? new DateTimeOffset(dt3, _TimeZone.GetUtcOffset(dt3)) : throw new(),
			Bet: json["gold"]!.GetValue<decimal>(),
			ValidBet: json["realGold"]!.GetValue<decimal>(),
			Settle: json["realGold"]!.GetValue<decimal>() + json["winGold"]!.GetValue<decimal>(),
			WinLoss: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeService = Substitute.For<IQueryService<GameRecordSportsSettlementQuery, GameRecordSportsResult>>();
		fakeService.QueryAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(new[] { serviceResponseItem }.ToAsyncEnumerable());
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IQueryService<GameRecordSportsSettlementQuery, GameRecordSportsResult>)));
				services.AddSingleton(Substitute.For<IGameApiClient>());
				services.AddSingleton(fakeService);
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceSharpSports.GameInterfaceSharpSportsClient(channel);
		var sut = new GameInterfaceSharpSportsClient(client);

		var actual = await sut.GameRecordSettlementAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			merchantId: grpcRequest.MerchantId,
			clientId: grpcRequest.ClientId,
			clientSecret: grpcRequest.ClientSecret,
			startTime: grpcRequest.StartTime.ToDateTimeOffset(),
			endTime: grpcRequest.EndTime.ToDateTimeOffset(),
			page: grpcRequest.Page,
			pageSize: grpcRequest.PageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("01HZ1F630052YMYPME9ZRVWKM7", item.ExternalBetId);
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.Equal(GameRecordState.Win, item.State);
		Assert.False(item.IsCashout);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:00"), TimeSpan.FromHours(8)), item.ExternalCreatedOn.ToDateTimeOffset());
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:10"), TimeSpan.FromHours(8)), item.ExternalClosedOn.ToDateTimeOffset());
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:12"), TimeSpan.FromHours(8)), item.ExternalPayoffOn.ToDateTimeOffset());
		Assert.Equal(10m, (decimal)item.Bet);
		Assert.Equal(10m, (decimal)item.ValidBet);
		Assert.Equal(12m, (decimal)item.Settle);
		Assert.Equal(2m, (decimal)item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task GameRecordSportsUnsettlement()
	{
		var grpcRequest = new GameRecordRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime = new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			EndTime = new DateTimeOffset(2024, 05, 29, 13, 00, 00, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			Page = 1,
			PageSize = 200,
		};
		var serviceRequest = new GameRecordSportsUnsettlementQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			ClientSecret: grpcRequest.ClientSecret,
			StartTime: grpcRequest.StartTime.ToDateTimeOffset(),
			EndTime: grpcRequest.EndTime.ToDateTimeOffset(),
			Page: grpcRequest.Page,
			PageSize: grpcRequest.PageSize);
		var json = new JsonObject
		{
			["merId"] = "c6a33e4e235e731b51d2f303199edeb4",
			["merName"] = "PG-CNY",
			["playerId"] = "506763ccb6d328049ccc7db080514a06",
			["loginName"] = "4ucmqplxb029",
			["playerName"] = "AliFardydon0Ty",
			["remoteIp"] = "125.227.69.109",
			["orderId"] = "QXT24060509004433396",
			["parlayNum"] = "1",
			["gold"] = 10,
			["realGold"] = 10,
			["winGold"] = null,
			["state"] = 1,
			["creditState"] = 0,
			["realResultDate"] = null,
			["resultDate"] = "",
			["resultDetail"] = null,
			["createDate"] = "2024-06-05 09:00:44",
			["terminalType"] = "16",
			["oddfType"] = "H",
			["betDTOList"] = new JsonArray {
				new JsonObject{
					["showType"] = "FU",
					["gameType"] = "FT",
					["playType"] = "R",
					["ratioType"] = "RH",
					["gameDate"] = "2024-06-14",
					["gameTime"] = "06:00:00",
					["awayTeam"] = "贝尔格拉诺",
					["homeTeam"] = "堤格雷",
					["leagueName"] = "阿根廷甲级联赛",
					["leagueShortName"] = "阿甲",
					["score"] = "",
					["ioRatio"] = "1.09",
					["ratio"] = "0",
					["resultScore"] = null,
					["betItem"] = "堤格雷 0",
					["betResultDetail"] = null,
					["session"] = null,
					["teamSuffix"] = "",
					["awayLangTeam"] = "{\"en-us\":\"CA Belgrano\",\"zh-tw\":\"貝爾格拉諾\",\"zh-cn\":\"贝尔格拉诺\",\"vi-vn\":\"\",\"th-th\":\"\"}",
					["homeLangTeam"] = "{\"en-us\":\"Club Atletico Tigre\",\"zh-tw\":\"堤格雷\",\"zh-cn\":\"堤格雷\",\"vi-vn\":\"Club Atletico Tigre\",\"th-th\":\"Club Atletico Tigre\"}",
					["leagueLangName"] = "{\"en-us\":\"Argentine Primera División\",\"zh-tw\":\"阿根廷足球甲級聯賽\",\"zh-cn\":\"阿根廷甲级联赛\",\"vi-vn\":\"Argentina Superliga\",\"th-th\":\"Argentina Superliga\"}",
					["leagueShortLangName"] = "{\"en-us\":\"ARG Primera División\",\"zh-tw\":\"阿甲\",\"zh-cn\":\"阿甲\",\"vi-vn\":\"Argentina Superliga\",\"th-th\":\"Argentina Superliga\"}",
					["betItem1"] = "Club Atletico Tigre 0",
					["playNameLang"] = "{\"en-us\":\"Handicap\",\"zh-tw\":\"讓球\",\"zh-cn\":\"让球\",\"vi-vn\":\"Cược chấ\",\"th-th\":\"แฮนดิแคป \"}",
					["playName"] = "让球",
					["betItemLang"] = "{\"en-us\":\"Club Atletico Tigre 0\",\"zh-tw\":\"堤格雷 0\",\"zh-cn\":\"堤格雷 0\",\"vi-vn\":\"Club Atletico Tigre 0\",\"th-th\":\"Club Atletico Tigre 0\"}",
					["gameId"] = "5859428086331637761495891",
					["gameTypeSon"] = null
				}
			},
			["lineType"] = "2",
			["cashoutType"] = "0",
			["cashouts"] = new JsonObject
			{
				["cashoutId"] = null,
				["stake"] = null,
				["initGold"] = null,
				["returnAmount"] = null,
				["wlAmount"] = null,
				["placeDateTime"] = null,
				["state"] = null
			},
			["betNum"] = 1,
			["lang"] = "zh-tw",
			["walletId"] = 1,
			["validAmount"] = null
		};
		var serviceResponseItem = new GameRecordSportsResult(
			ExternalBetId: "QXT24060509004433396",
			PlayerCode: "4ucmqplxb029",
			State: SportOrderListState.Running,
			IsCashout: false,
			ExternalCreatedOn: new DateTimeOffset(DateTime.Parse("2024-06-05 09:00:44"), TimeSpan.FromHours(8)),
			ExternalClosedOn: DateTime.UnixEpoch,
			ExternalPayoffOn: DateTime.UnixEpoch,
			Bet: 10m,
			ValidBet: 10m,
			Settle: 0.0m,
			WinLoss: 0.0m,
			AdditionalData: json.ToJsonString());
		var fakeService = Substitute.For<IQueryService<GameRecordSportsUnsettlementQuery, GameRecordSportsResult>>();
		fakeService.QueryAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(new[] { serviceResponseItem }.ToAsyncEnumerable());
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IQueryService<GameRecordSportsUnsettlementQuery, GameRecordSportsResult>)));
				services.AddSingleton(Substitute.For<IGameApiClient>());
				services.AddSingleton(fakeService);
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceSharpSports.GameInterfaceSharpSportsClient(channel);
		var sut = new GameInterfaceSharpSportsClient(client);

		var actual = await sut.GameRecordUnsettlementAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			merchantId: grpcRequest.MerchantId,
			clientId: grpcRequest.ClientId,
			clientSecret: grpcRequest.ClientSecret,
			startTime: grpcRequest.StartTime.ToDateTimeOffset(),
			endTime: grpcRequest.EndTime.ToDateTimeOffset(),
			page: grpcRequest.Page,
			pageSize: grpcRequest.PageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("QXT24060509004433396", item.ExternalBetId);
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.Equal(GameRecordState.Running, item.State);
		Assert.False(item.IsCashout);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-05 09:00:44"), TimeSpan.FromHours(8)), item.ExternalCreatedOn.ToDateTimeOffset());
		Assert.Equal(DateTime.UnixEpoch, item.ExternalClosedOn.ToDateTimeOffset());
		Assert.Equal(DateTime.UnixEpoch, item.ExternalPayoffOn.ToDateTimeOffset());
		Assert.Equal(10m, (decimal)item.Bet);
		Assert.Equal(10m, (decimal)item.ValidBet);
		Assert.Equal(0.0m, (decimal)item.Settle);
		Assert.Equal(0.0m, (decimal)item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}
}
