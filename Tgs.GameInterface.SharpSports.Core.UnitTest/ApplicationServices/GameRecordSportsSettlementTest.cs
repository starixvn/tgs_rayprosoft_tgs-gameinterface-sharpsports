using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;

using Tgs.GameInterface.SharpSports.Core.Models;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest.ApplicationServices;

public sealed class GameRecordSportsSettlementTest
{
	private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

	[Fact]
	public async Task Normal_Process()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = 0,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.SecretKey),
				beginTime: Arg.Is(query.StartTime),
				endTime: Arg.Is(query.EndTime),
				timeType: Arg.Is(SportOrderListTimeType.ResultDate),
				pageNum: Arg.Is(1),
				pageSize: Arg.Is(200))
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("01HZ13ZFAS9M5HF75CKFX5YE2R", data.ExternalBetId);
		Assert.Equal("4ucmqplxb029", data.PlayerCode);
		Assert.False(data.IsClosed);
		Assert.False(data.IsCancel);
		Assert.False(data.IsRefund);
		Assert.False(data.IsVoid);
		Assert.False(data.IsCashout);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 10:50:01"), TimeSpan.FromHours(8)), data.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 10:50:03"), TimeSpan.FromHours(8)), data.ExternalClosedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-05-29 10:50:04"), TimeSpan.FromHours(8)), data.ExternalPayoffOn);
		Assert.Equal(10m, data.Bet);
		Assert.Equal(10m, data.ValidBet);
		Assert.Equal(12m, data.Settle);
		Assert.Equal(2m, data.WinLoss);
		Assert.Equal(json.ToJsonString(), data.AdditionalData);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	public async Task IsClosedFalse_WhenApiState0Or1(int state)
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = state,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.False(data.IsClosed);
	}

	[Theory]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	public async Task IsClosedTrue_WhenApiStateNot0Not1(int state)
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = state,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.True(data.IsClosed);
	}

	[Theory]
	[InlineData(5)]
	[InlineData(7)]
	public async Task IsCancelTrue_WhenApiState5Or7(int state)
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = state,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.True(data.IsCancel);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(6)]
	[InlineData(8)]
	public async Task IsCancelFalse_WhenApiStateNot5Not7(int state)
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = state,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.False(data.IsCancel);
	}

	[Fact]
	public async Task IsRefundTrue_WhenApiState6()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = 6,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.True(data.IsRefund);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(7)]
	[InlineData(8)]
	public async Task IsRefundFalse_WhenApiStateNot6(int state)
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = state,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.False(data.IsRefund);
	}

	[Fact]
	public async Task IsVoidTrue_WhenApiState8()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = 8,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.True(data.IsVoid);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	public async Task IsVoidFalse_WhenApiStateNot8(int state)
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = state,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.False(data.IsVoid);
	}

	[Fact]
	public async Task IsCashoutFalse_WhenApiCashoutType0()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = 0,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
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
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.False(data.IsCashout);
	}

	[Theory]
	[InlineData("1")]
	[InlineData("2")]
	public async Task IsCashoutTrue_WhenApiCashoutTypeNot0(string cashoutType)
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = 0,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
			["gold"] = 10m,
			["realGold"] = 10m,
			["winGold"] = 2m,
			["cashoutType"] = cashoutType,
		};
		var responseItem = new SportOrderListResponseItem(
			LoginName: json["loginName"]!.GetValue<string>(),
			OrderId: json["orderId"]!.GetValue<string>(),
			State: json["state"]!.GetValue<int>(),
			CashoutType: json["cashoutType"]!.GetValue<string>(),
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.True(data.IsCashout);
	}

	[Fact]
	public async Task CalculateSettle()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["loginName"] = "4ucmqplxb029",
			["orderId"] = "01HZ13ZFAS9M5HF75CKFX5YE2R",
			["state"] = 3,
			["createDate"] = "2024-05-29 10:50:01",
			["resultDate"] = "2024-05-29 10:50:03",
			["realResultDate"] = "2024-05-29 10:50:04",
			["gold"] = 10m,
			["realGold"] = 10m,
			["winGold"] = 3m,
			["cashoutType"] = "0",
		};
		var responseItem = new SportOrderListResponseItem(
			LoginName: json["loginName"]!.GetValue<string>(),
			OrderId: json["orderId"]!.GetValue<string>(),
			State: json["state"]!.GetValue<int>(),
			CashoutType: json["cashoutType"]!.GetValue<string>(),
			CreateDate: DateTime.TryParse(json["createDate"]!.GetValue<string>(), out var createDate) ? new(createDate, _TimeZone.GetUtcOffset(createDate)) : DateTimeOffset.UnixEpoch,
			ResultDate: DateTime.TryParse(json["resultDate"]!.GetValue<string>(), out var resultDate) ? new(resultDate, _TimeZone.GetUtcOffset(resultDate)) : DateTimeOffset.UnixEpoch,
			RealResultDate: DateTime.TryParse(json["realResultDate"]!.GetValue<string>(), out var rrDate) ? new(rrDate, _TimeZone.GetUtcOffset(rrDate)) : DateTimeOffset.UnixEpoch,
			Gold: json["gold"]!.GetValue<decimal>(),
			RealGold: json["realGold"]!.GetValue<decimal>(),
			WinGold: json["winGold"]!.GetValue<decimal>(),
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				Arg.Any<Uri>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<DateTimeOffset>(),
				Arg.Any<SportOrderListTimeType>(),
				Arg.Any<int>(),
				Arg.Any<int>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal(13m, data.Settle);
	}
}
