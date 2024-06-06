using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.SharpSports.Client;
using Tgs.GameInterface.SharpSports.Core;
using Tgs.GameInterface.SharpSports.Protos;
using Tgs.ServiceUtility;
using Xunit;
using ServiceModels = Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;

namespace Tgs.GameInterface.SharpSports.IntegrationTest;

public sealed class GameInterfaceSharpSportsGrpcServiceTests
{
	private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

	[Fact]
	public async void CheckingService()
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
	public async void GameUrl()
	{
		var grpcRequest = new GameUrlRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
			IsMobile = true,
			Language = "zh-cn",
			Ip = "127.0.0.1",
		};
		var serviceRequest = new ServiceModels.GameUrlSportsQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			SecretKey: grpcRequest.SecretKey,
			PlayerCode: grpcRequest.PlayerCode,
			IsMobile: grpcRequest.IsMobile,
			Language: grpcRequest.Language,
			Ip: grpcRequest.Ip);
		var serviceResponse = new ServiceModels.GameUrlSportsResult(
			GameUrl: "https://xxx.xxx.xxx/xxx");
		var fakeService = Substitute.For<IGetService<ServiceModels.GameUrlSportsQuery, ServiceModels.GameUrlSportsResult>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<ServiceModels.GameUrlSportsQuery, ServiceModels.GameUrlSportsResult>)));
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
			secretKey: grpcRequest.SecretKey,
			playerCode: grpcRequest.PlayerCode,
			isMobile: grpcRequest.IsMobile,
			language: grpcRequest.Language,
			ip: grpcRequest.Ip)
			.ConfigureAwait(true);

		Assert.Equal("https://xxx.xxx.xxx/xxx", actual.GameUrl);
	}

	[Fact]
	public async void PlayerBalance()
	{
		var grpcRequest = new PlayerBalanceRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
		};
		var serviceRequest = new ServiceModels.PlayerBalanceQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			SecretKey: grpcRequest.SecretKey,
			PlayerCode: grpcRequest.PlayerCode);
		var serviceResponse = new ServiceModels.PlayerBalanceResult(
			Balance: 10m);
		var fakeService = Substitute.For<IGetService<ServiceModels.PlayerBalanceQuery, ServiceModels.PlayerBalanceResult>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<ServiceModels.PlayerBalanceQuery, ServiceModels.PlayerBalanceResult>)));
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
			secretKey: grpcRequest.SecretKey,
			playerCode: grpcRequest.PlayerCode)
			.ConfigureAwait(true);

		Assert.Equal(10m, (decimal)actual.Balance);
	}

	[Fact]
	public async void FundIn()
	{
		var grpcRequest = new FundRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
			OrderId = "01HZ1GGQFKP2278WJVBVGYT7NX",
			Amount = 6m,
		};
		var serviceRequest = new ServiceModels.FundInQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			SecretKey: grpcRequest.SecretKey,
			PlayerCode: grpcRequest.PlayerCode,
			OrderId: grpcRequest.OrderId,
			Amount: grpcRequest.Amount);
		var json = new JsonObject
		{
			["tradeNo"] = "01HZ1G8FW3KM1W10NZFMNCVE6C",
			["merchantPayNo"] = grpcRequest.OrderId,
			["balance"] = 10m,
		};
		var serviceResponse = new ServiceModels.FundResult(
			ExternalOrderId: json["tradeNo"]!.GetValue<string>(),
			OrderId: json["merchantPayNo"]!.GetValue<string>(),
			Balance: json["balance"]!.GetValue<decimal>(),
			Resp: json.ToJsonString());
		var fakeService = Substitute.For<IGetService<ServiceModels.FundInQuery, ServiceModels.FundResult>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<ServiceModels.FundInQuery, ServiceModels.FundResult>)));
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
			secretKey: grpcRequest.SecretKey,
			playerCode: grpcRequest.PlayerCode,
			orderId: grpcRequest.OrderId,
			amount: grpcRequest.Amount)
			.ConfigureAwait(true);

		Assert.Equal(10m, (decimal)actual.Balance);
		Assert.Equal(json.ToJsonString(), actual.Resp);
	}

	[Fact]
	public async void FundOut()
	{
		var grpcRequest = new FundRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
			OrderId = "01HZ1GGQFKP2278WJVBVGYT7NX",
			Amount = 6m,
		};
		var serviceRequest = new ServiceModels.FundOutQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			SecretKey: grpcRequest.SecretKey,
			PlayerCode: grpcRequest.PlayerCode,
			OrderId: grpcRequest.OrderId,
			Amount: grpcRequest.Amount);
		var json = new JsonObject
		{
			["tradeNo"] = "01HZ1G8FW3KM1W10NZFMNCVE6C",
			["merchantPayNo"] = grpcRequest.OrderId,
			["balance"] = 10m,
		};
		var serviceResponse = new ServiceModels.FundResult(
			ExternalOrderId: json["tradeNo"]!.GetValue<string>(),
			OrderId: json["merchantPayNo"]!.GetValue<string>(),
			Balance: json["balance"]!.GetValue<decimal>(),
			Resp: json.ToJsonString());
		var fakeService = Substitute.For<IGetService<ServiceModels.FundOutQuery, ServiceModels.FundResult>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<ServiceModels.FundOutQuery, ServiceModels.FundResult>)));
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
			secretKey: grpcRequest.SecretKey,
			playerCode: grpcRequest.PlayerCode,
			orderId: grpcRequest.OrderId,
			amount: grpcRequest.Amount)
			.ConfigureAwait(true);

		Assert.Equal(10m, (decimal)actual.Balance);
		Assert.Equal(json.ToJsonString(), actual.Resp);
	}

	[Fact]
	public async void FundStatus()
	{
		var grpcRequest = new FundStatusRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode = "4ucmqplxb029",
			OrderOn = new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			OrderId = "01HZ1GM13J5QZGXGH4JW7AJFXT",
		};
		var serviceRequest = new ServiceModels.FundStatusQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			SecretKey: grpcRequest.SecretKey,
			PlayerCode: grpcRequest.PlayerCode,
			OrderOn: grpcRequest.OrderOn.ToDateTimeOffset(),
			OrderId: grpcRequest.OrderId);
		var serviceResponse = true;
		var fakeService = Substitute.For<IGetService<ServiceModels.FundStatusQuery, bool>>();
		fakeService.GetAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IGetService<ServiceModels.FundStatusQuery, bool>)));
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
			secretKey: grpcRequest.SecretKey,
			playerCode: grpcRequest.PlayerCode,
			orderOn: grpcRequest.OrderOn.ToDateTimeOffset(),
			orderId: grpcRequest.OrderId)
			.ConfigureAwait(true);

		Assert.True(actual);
	}

	[Fact]
	public async void GameRecordSportsSettlement()
	{
		var grpcRequest = new GameRecordRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime = new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			EndTime = new DateTimeOffset(2024, 05, 29, 13, 00, 00, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			Page = 1,
			PageSize = 200,
		};
		var serviceRequest = new ServiceModels.GameRecordSportsSettlementQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			SecretKey: grpcRequest.SecretKey,
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
		var serviceResponseItem = new ServiceModels.GameRecordSportsGameBetSportsItem(
			ExternalBetId: json["orderId"]!.GetValue<string>(),
			PlayerCode: json["loginName"]!.GetValue<string>(),
			IsClosed: json["state"]!.GetValue<int>() is not (0 or 1),
			IsCancel: json["state"]!.GetValue<int>() is 5 or 7,
			IsRefund: json["state"]!.GetValue<int>() is 6,
			IsVoid: json["state"]!.GetValue<int>() is 8,
			IsCashout: json["cashoutType"]!.GetValue<string>() is not "0",
			ExternalCreatedOn: (DateTime.Parse(json["createDate"]!.GetValue<string>()) is DateTime dt1) ? new DateTimeOffset(dt1, _TimeZone.GetUtcOffset(dt1)) : throw new(),
			ExternalClosedOn: (DateTime.Parse(json["resultDate"]!.GetValue<string>()) is DateTime dt2) ? new DateTimeOffset(dt2, _TimeZone.GetUtcOffset(dt2)) : throw new(),
			ExternalPayoffOn: (DateTime.Parse(json["realResultDate"]!.GetValue<string>()) is DateTime dt3) ? new DateTimeOffset(dt3, _TimeZone.GetUtcOffset(dt3)) : throw new(),
			Bet: json["gold"]!.GetValue<decimal>(),
			ValidBet: json["realGold"]!.GetValue<decimal>(),
			Settle: json["realGold"]!.GetValue<decimal>() + json["winGold"]!.GetValue<decimal>(),
			WinLoss: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeService = Substitute.For<IQueryService<ServiceModels.GameRecordSportsSettlementQuery, ServiceModels.GameRecordSportsGameBetSportsItem>>();
		fakeService.QueryAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(new[] { serviceResponseItem }.ToAsyncEnumerable());
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IQueryService<ServiceModels.GameRecordSportsSettlementQuery, ServiceModels.GameRecordSportsGameBetSportsItem>)));
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
			secretKey: grpcRequest.SecretKey,
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
		Assert.True(item.IsClosed);
		Assert.False(item.IsCancel);
		Assert.False(item.IsRefund);
		Assert.False(item.IsVoid);
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
	public async void GameRecordSportsUnsettlement()
	{
		var grpcRequest = new GameRecordRequest
		{
			ApiUrl = "https://api.yuansports1.com/",
			MerchantId = "c6a33e4e235e731b51d2f303199edeb4",
			ClientId = "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey = "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime = new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			EndTime = new DateTimeOffset(2024, 05, 29, 13, 00, 00, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			Page = 1,
			PageSize = 200,
		};
		var serviceRequest = new ServiceModels.GameRecordSportsUnsettlementQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			MerchantId: grpcRequest.MerchantId,
			ClientId: grpcRequest.ClientId,
			SecretKey: grpcRequest.SecretKey,
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
		var serviceResponseItem = new ServiceModels.GameRecordSportsGameBetSportsItem(
			ExternalBetId: json["orderId"]!.GetValue<string>(),
			PlayerCode: json["loginName"]!.GetValue<string>(),
			IsClosed: json["state"]!.GetValue<int>() is not (0 or 1),
			IsCancel: json["state"]!.GetValue<int>() is 5 or 7,
			IsRefund: json["state"]!.GetValue<int>() is 6,
			IsVoid: json["state"]!.GetValue<int>() is 8,
			IsCashout: json["cashoutType"]!.GetValue<string>() is not "0",
			ExternalCreatedOn: (DateTime.Parse(json["createDate"]!.GetValue<string>()) is DateTime dt1) ? new DateTimeOffset(dt1, _TimeZone.GetUtcOffset(dt1)) : throw new(),
			ExternalClosedOn: (DateTime.Parse(json["resultDate"]!.GetValue<string>()) is DateTime dt2) ? new DateTimeOffset(dt2, _TimeZone.GetUtcOffset(dt2)) : throw new(),
			ExternalPayoffOn: (DateTime.Parse(json["realResultDate"]!.GetValue<string>()) is DateTime dt3) ? new DateTimeOffset(dt3, _TimeZone.GetUtcOffset(dt3)) : throw new(),
			Bet: json["gold"]!.GetValue<decimal>(),
			ValidBet: json["realGold"]!.GetValue<decimal>(),
			Settle: json["realGold"]!.GetValue<decimal>() + json["winGold"]!.GetValue<decimal>(),
			WinLoss: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeService = Substitute.For<IQueryService<ServiceModels.GameRecordSportsUnsettlementQuery, ServiceModels.GameRecordSportsGameBetSportsItem>>();
		fakeService.QueryAsync(Arg.Is(serviceRequest), Arg.Any<CancellationToken>()).Returns(new[] { serviceResponseItem }.ToAsyncEnumerable());
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.Remove(services.First(d => d.ServiceType == typeof(IQueryService<ServiceModels.GameRecordSportsUnsettlementQuery, ServiceModels.GameRecordSportsGameBetSportsItem>)));
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
			secretKey: grpcRequest.SecretKey,
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
		Assert.True(item.IsClosed);
		Assert.False(item.IsCancel);
		Assert.False(item.IsRefund);
		Assert.False(item.IsVoid);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:00"), TimeSpan.FromHours(8)), item.ExternalCreatedOn.ToDateTimeOffset());
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:10"), TimeSpan.FromHours(8)), item.ExternalClosedOn.ToDateTimeOffset());
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:12"), TimeSpan.FromHours(8)), item.ExternalPayoffOn.ToDateTimeOffset());
		Assert.Equal(10m, (decimal)item.Bet);
		Assert.Equal(10m, (decimal)item.ValidBet);
		Assert.Equal(12m, (decimal)item.Settle);
		Assert.Equal(2m, (decimal)item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}
}
