using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest;

public sealed class GameApiClient_SportOrderListTest
{
	private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

	[Fact]
	public async Task Normal_Process()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var beginTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var timeType = SportOrderListTimeType.CreateDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["timeType"] = (byte)timeType,
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
		};
		var encoded = "78A8C3AA709E882C2570513843C85264EC00534CAD6DE435CD71F14438912451250C2B0C9BDDBE4B0EFE25A58E801D22EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DA1D95A8F7129A914A617199C058B165C0FBAC0238E0B0C6E7A53623F89FD3ED2D";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["orderDTO"] = new JsonArray
				{
					new JsonObject
					{
						["loginName"] = "4ucmqplxb029",
						["orderId"] = "01HZ1MJAQ2EYEJWWVEY3M17X91",
						["state"] = 3,
						["createDate"] = "2024-05-29 15:40:00",
						["resultDate"] = "2024-05-29 15:40:02",
						["realResultDate"] = "2024-05-29 15:40:03",
						["gold"] = 10m,
						["realGold"] = 10m,
						["winGold"] = 2m,
						["cashoutType"] = "0",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/sport/orderList").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["beginTime"]!.GetValue<string>() == parameters["beginTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["timeType"]!.GetValue<byte>() == parameters["timeType"]!.GetValue<byte>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.SportOrderListAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			beginTime,
			endTime,
			timeType,
			pageNum,
			pageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("4ucmqplxb029", data.LoginName);
		Assert.Equal("01HZ1MJAQ2EYEJWWVEY3M17X91", data.OrderId);
		Assert.Equal(3, data.State);
		Assert.Equal(new DateTimeOffset(2024, 05, 29, 15, 40, 00, TimeSpan.FromHours(8)), data.CreateDate);
		Assert.Equal(new DateTimeOffset(2024, 05, 29, 15, 40, 02, TimeSpan.FromHours(8)), data.ResultDate);
		Assert.Equal(new DateTimeOffset(2024, 05, 29, 15, 40, 03, TimeSpan.FromHours(8)), data.RealResultDate);
		Assert.Equal(10m, data.Gold);
		Assert.Equal(10m, data.RealGold);
		Assert.Equal(2m, data.WinGold);
		Assert.Equal(response["data"]!["orderDTO"]!.AsArray()[0]!.ToJsonString(), data.AdditionalData);
	}

	[Fact]
	public async Task ApiResponse_ResultDateEmpty_RealResultDateNull_WinGoldNull_ServiceReturn_ResultDateUnixEpoch_RealResultDateUnixEpoch_WinGold0()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var beginTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var timeType = SportOrderListTimeType.CreateDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["timeType"] = (byte)timeType,
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
		};
		var encoded = "78A8C3AA709E882C2570513843C85264EC00534CAD6DE435CD71F14438912451250C2B0C9BDDBE4B0EFE25A58E801D22EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DA1D95A8F7129A914A617199C058B165C0FBAC0238E0B0C6E7A53623F89FD3ED2D";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["orderDTO"] = new JsonArray
				{
					new JsonObject
					{
						["loginName"] = "4ucmqplxb029",
						["orderId"] = "01HZ1MJAQ2EYEJWWVEY3M17X91",
						["state"] = 3,
						["createDate"] = "2024-05-29 15:40:00",
						["resultDate"] = "",
						["realResultDate"] = null,
						["gold"] = 10m,
						["realGold"] = 10m,
						["winGold"] = null,
						["cashoutType"] = "0",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/sport/orderList").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["beginTime"]!.GetValue<string>() == parameters["beginTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["timeType"]!.GetValue<byte>() == parameters["timeType"]!.GetValue<byte>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.SportOrderListAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			beginTime,
			endTime,
			timeType,
			pageNum,
			pageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("4ucmqplxb029", data.LoginName);
		Assert.Equal("01HZ1MJAQ2EYEJWWVEY3M17X91", data.OrderId);
		Assert.Equal(3, data.State);
		Assert.Equal(new DateTimeOffset(2024, 05, 29, 15, 40, 00, TimeSpan.FromHours(8)), data.CreateDate);
		Assert.Equal(DateTimeOffset.UnixEpoch, data.ResultDate);
		Assert.Equal(DateTimeOffset.UnixEpoch, data.RealResultDate);
		Assert.Equal(10m, data.Gold);
		Assert.Equal(10m, data.RealGold);
		Assert.Equal(0m, data.WinGold);
		Assert.Equal(response["data"]!["orderDTO"]!.AsArray()[0]!.ToJsonString(), data.AdditionalData);
	}

	[Theory]
	[InlineData(400100)]
	[InlineData(400101)]
	[InlineData(400103)]
	[InlineData(400109)]
	[InlineData(400200)]
	[InlineData(400201)]
	[InlineData(400202)]
	[InlineData(400104)]
	[InlineData(400205)]
	[InlineData(400208)]
	[InlineData(400209)]
	public async Task ThrowErrorCodeException_WhenStatusCodeNot200(int code)
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var beginTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var timeType = SportOrderListTimeType.CreateDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["timeType"] = (byte)timeType,
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
		};
		var encoded = "78A8C3AA709E882C2570513843C85264EC00534CAD6DE435CD71F14438912451250C2B0C9BDDBE4B0EFE25A58E801D22EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DA1D95A8F7129A914A617199C058B165C0FBAC0238E0B0C6E7A53623F89FD3ED2D";
		var response = new JsonObject
		{
			["code"] = code,
			["data"] = null,
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/sport/orderList").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["beginTime"]!.GetValue<string>() == parameters["beginTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["timeType"]!.GetValue<byte>() == parameters["timeType"]!.GetValue<byte>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var exception = await Assert.ThrowsAsync<ErrorCodeException>(
			() => sut.SportOrderListAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			beginTime,
			endTime,
			timeType,
			pageNum,
			pageSize).ToArrayAsync().AsTask())
			.ConfigureAwait(true);

		Assert.Equal(code, exception.Code);
	}
}
