using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.SharpSports.Core;
using Tgs.GameInterface.SharpSports.Core.Models;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;
using Xunit;

namespace Tgs.GameInterface.SharpSports.IntegrationTest;

public sealed class SharpSportsControllerTests
{
	private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

	[Fact]
	public async void GameUrlSports()
	{
		var query = new GameUrlSportsQuery(
			ApiUrl: new Uri("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029",
			IsMobile: false,
			Language: "zh-cn",
			Ip: "127.0.0.1");
		var response = "https://xxx.xxx.xxx/xxx";
		var apiClient = Substitute.For<IGameApiClient>();
		_ = apiClient.PlayerSportsLoginAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.SecretKey),
				loginName: Arg.Is(query.PlayerCode),
				terType: Arg.Is(query.IsMobile ? TerType.H5 : TerType.PC),
				language: Arg.Is(query.Language),
				ip: Arg.Is(query.Ip))
			.Returns(ValueTask.FromResult(response));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.AddSingleton(apiClient);
			}));
		var sut = factory.CreateClient();

		var actual = await sut.PostAsync("/SharpSports/GameUrlSports", JsonContent.Create(query)).ConfigureAwait(true);

		actual.EnsureSuccessStatusCode();
		var result = await actual.Content.ReadFromJsonAsync<GameUrlSportsResult>().ConfigureAwait(true);
		Assert.NotNull(result);
		Assert.Equal("https://xxx.xxx.xxx/xxx", result.GameUrl);
	}

	[Fact]
	public async void PlayerBalance()
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029");
		var response = 10m;
		var apiClient = Substitute.For<IGameApiClient>();
		_ = apiClient.PlayerBalanceAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.SecretKey),
				loginName: Arg.Is(query.PlayerCode))
			.Returns(ValueTask.FromResult(response));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.AddSingleton(apiClient);
			}));
		var sut = factory.CreateClient();

		var actual = await sut.PostAsync($"/SharpSports/PlayerBalance", JsonContent.Create(query)).ConfigureAwait(true);

		actual.EnsureSuccessStatusCode();
		var result = await actual.Content.ReadFromJsonAsync<PlayerBalanceResult>().ConfigureAwait(true);
		Assert.NotNull(result);
		Assert.Equal(10m, result.Balance);
	}

	[Fact]
	public async void FundIn()
	{
		var query = new FundInQuery(
			ApiUrl: new Uri("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029",
			OrderId: "01HZ1GGQFKP2278WJVBVGYT7NX",
			Amount: 6m);
		var json = new JsonObject
		{
			["tradeNo"] = "01HZ1G8FW3KM1W10NZFMNCVE6C",
			["merchantPayNo"] = query.OrderId,
			["balance"] = 10m,
		};
		var response = new WalletDepositAndWithDrawResponse(
			TradeNo: json["tradeNo"]!.GetValue<string>(),
			MerchantPayNo: json["merchantPayNo"]!.GetValue<string>(),
			Balance: json["balance"]!.GetValue<decimal>(),
			Resp: json.ToJsonString());
		var apiClient = Substitute.For<IGameApiClient>();
		_ = apiClient.WalletDepositAndWithDrawAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.SecretKey),
				loginName: Arg.Is(query.PlayerCode),
				merchantPayNo: Arg.Is(query.OrderId),
				tradeType: Arg.Is(TradeType.WithDraw),
				amount: Arg.Is(query.Amount))
			.Returns(ValueTask.FromResult(response));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.AddSingleton(apiClient);
			}));
		var sut = factory.CreateClient();

		var actual = await sut.PostAsync("/SharpSports/FundIn", JsonContent.Create(query)).ConfigureAwait(true);

		actual.EnsureSuccessStatusCode();
		var result = await actual.Content.ReadFromJsonAsync<FundResult>().ConfigureAwait(true);
		Assert.NotNull(result);
		Assert.Equal(10m, result.Balance);
		Assert.Equal(json.ToJsonString(), result.Resp);
	}

	[Fact]
	public async void FundOut()
	{
		var query = new FundOutQuery(
			ApiUrl: new Uri("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029",
			OrderId: "01HZ1GGQFKP2278WJVBVGYT7NX",
			Amount: 6m);
		var json = new JsonObject
		{
			["tradeNo"] = "01HZ1GH712GJBHCT1ZQYZ8Q67Y",
			["merchantPayNo"] = query.OrderId,
			["balance"] = 10m,
		};
		var response = new WalletDepositAndWithDrawResponse(
			TradeNo: json["tradeNo"]!.GetValue<string>(),
			MerchantPayNo: json["merchantPayNo"]!.GetValue<string>(),
			Balance: json["balance"]!.GetValue<decimal>(),
			Resp: json.ToJsonString());
		var apiClient = Substitute.For<IGameApiClient>();
		_ = apiClient.WalletDepositAndWithDrawAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.SecretKey),
				loginName: Arg.Is(query.PlayerCode),
				merchantPayNo: Arg.Is(query.OrderId),
				tradeType: Arg.Is(TradeType.Deposit),
				amount: Arg.Is(query.Amount))
			.Returns(ValueTask.FromResult(response));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.AddSingleton(apiClient);
			}));
		var sut = factory.CreateClient();

		var actual = await sut.PostAsync("/SharpSports/FundOut", JsonContent.Create(query)).ConfigureAwait(true);

		actual.EnsureSuccessStatusCode();
		var result = await actual.Content.ReadFromJsonAsync<FundResult>().ConfigureAwait(true);
		Assert.NotNull(result);
		Assert.Equal(10m, result.Balance);
		Assert.Equal(json.ToJsonString(), result.Resp);
	}

	[Fact]
	public async void FundStatus()
	{
		var query = new FundStatusQuery(
			ApiUrl: new Uri("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029",
			OrderOn: new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)),
			OrderId: "01HZ1GM13J5QZGXGH4JW7AJFXT");
		var apiClient = Substitute.For<IGameApiClient>();
		_ = apiClient.TransferRecordsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.SecretKey),
				loginName: Arg.Is(query.PlayerCode),
				startTime: Arg.Is(query.OrderOn.AddHours(-6)),
				endTime: Arg.Is(query.OrderOn.Add(new(hours: 5, minutes: 59, seconds: 59))),
				pageNum: Arg.Is(1),
				pageSize: Arg.Is(200),
				merchantPayNo: Arg.Is(query.OrderId))
			.Returns(ValueTask.FromResult(true));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.AddSingleton(apiClient);
			}));
		var sut = factory.CreateClient();

		var actual = await sut.PostAsync("/SharpSports/FundStatus", JsonContent.Create(query)).ConfigureAwait(true);

		actual.EnsureSuccessStatusCode();
		var result = await actual.Content.ReadFromJsonAsync<JsonNode>().ConfigureAwait(true);
		Assert.NotNull(result);
		Assert.True(result.GetValue<bool>());
	}

	[Fact]
	public async void GameRecordSportsSettlement()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new Uri("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 13, 00, 00, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
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
		var responseItem = new SportOrderListResponseItem(
			LoginName: json["loginName"]!.GetValue<string>(),
			OrderId: json["orderId"]!.GetValue<string>(),
			State: json["state"]!.GetValue<int>(),
			CashoutType: json["cashoutType"]!.GetValue<string>(),
			CreateDate: (DateTime.Parse(json["createDate"]!.GetValue<string>()) is DateTime dt1) ? new DateTimeOffset(dt1, _TimeZone.GetUtcOffset(dt1)) : throw new(),
			ResultDate: (DateTime.Parse(json["resultDate"]!.GetValue<string>()) is DateTime dt2) ? new DateTimeOffset(dt2, _TimeZone.GetUtcOffset(dt2)) : throw new(),
			RealResultDate: (DateTime.Parse(json["realResultDate"]!.GetValue<string>()) is DateTime dt3) ? new DateTimeOffset(dt3, _TimeZone.GetUtcOffset(dt3)) : throw new(),
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var apiClient = Substitute.For<IGameApiClient>();
		_ = apiClient.SportOrderListAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.SecretKey),
				beginTime: Arg.Is(query.StartTime),
				endTime: Arg.Is(query.EndTime),
				timeType: Arg.Is(SportOrderListTimeType.ResultDate),
				pageNum: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize))
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.AddSingleton(apiClient);
			}));
		var sut = factory.CreateClient();

		var actual = await sut.PostAsync("/SharpSports/GameRecordSportsSettlement", JsonContent.Create(query)).ConfigureAwait(true);

		actual.EnsureSuccessStatusCode();
		var result = await actual.Content.ReadFromJsonAsync<GameRecordSportsGameBetSportsItem[]>().ConfigureAwait(true);
		Assert.NotNull(result);
		Assert.Single(result);
		var item = result[0];
		Assert.Equal("01HZ1F630052YMYPME9ZRVWKM7", item.ExternalBetId);
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.True(item.IsClosed);
		Assert.False(item.IsCancel);
		Assert.False(item.IsRefund);
		Assert.False(item.IsVoid);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:00"), TimeSpan.FromHours(8)), item.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:10"), TimeSpan.FromHours(8)), item.ExternalClosedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:12"), TimeSpan.FromHours(8)), item.ExternalPayoffOn);
		Assert.Equal(10m, item.Bet);
		Assert.Equal(10m, item.ValidBet);
		Assert.Equal(12m, item.Settle);
		Assert.Equal(2m, item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async void GameRecordSportsUnsettlement()
	{
		var query = new GameRecordSportsUnsettlementQuery(
			ApiUrl: new Uri("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 28, 14, 59, 59, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 13, 00, 00, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
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
		var responseItem = new SportOrderListResponseItem(
			LoginName: json["loginName"]!.GetValue<string>(),
			OrderId: json["orderId"]!.GetValue<string>(),
			State: json["state"]!.GetValue<int>(),
			CashoutType: json["cashoutType"]!.GetValue<string>(),
			CreateDate: (DateTime.Parse(json["createDate"]!.GetValue<string>()) is DateTime dt1) ? new DateTimeOffset(dt1, _TimeZone.GetUtcOffset(dt1)) : throw new(),
			ResultDate: (DateTime.Parse(json["resultDate"]!.GetValue<string>()) is DateTime dt2) ? new DateTimeOffset(dt2, _TimeZone.GetUtcOffset(dt2)) : throw new(),
			RealResultDate: (DateTime.Parse(json["realResultDate"]!.GetValue<string>()) is DateTime dt3) ? new DateTimeOffset(dt3, _TimeZone.GetUtcOffset(dt3)) : throw new(),
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var apiClient = Substitute.For<IGameApiClient>();
		_ = apiClient.SportOrderListAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.SecretKey),
				beginTime: Arg.Is(query.StartTime),
				endTime: Arg.Is(query.EndTime),
				timeType: Arg.Is(SportOrderListTimeType.CreateDate),
				pageNum: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize))
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Remove(services.First(d => d.ServiceType == typeof(IGameApiClient)));
				services.AddSingleton(apiClient);
			}));
		var sut = factory.CreateClient();

		var actual = await sut.PostAsync("/SharpSports/GameRecordSportsUnsettlement", JsonContent.Create(query)).ConfigureAwait(true);

		actual.EnsureSuccessStatusCode();
		var result = await actual.Content.ReadFromJsonAsync<GameRecordSportsGameBetSportsItem[]>().ConfigureAwait(true);
		Assert.NotNull(result);
		Assert.Single(result);
		var item = result[0];
		Assert.Equal("01HZ1F630052YMYPME9ZRVWKM7", item.ExternalBetId);
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.True(item.IsClosed);
		Assert.False(item.IsCancel);
		Assert.False(item.IsRefund);
		Assert.False(item.IsVoid);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:00"), TimeSpan.FromHours(8)), item.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:10"), TimeSpan.FromHours(8)), item.ExternalClosedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 01:00:12"), TimeSpan.FromHours(8)), item.ExternalPayoffOn);
		Assert.Equal(10m, item.Bet);
		Assert.Equal(10m, item.ValidBet);
		Assert.Equal(12m, item.Settle);
		Assert.Equal(2m, item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}
}
