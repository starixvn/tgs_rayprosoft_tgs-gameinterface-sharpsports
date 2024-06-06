using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest;

public sealed class GameApiClient_TransferRecordsTest
{
	private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");

	[Fact]
	public async Task TransferRecordsFound_WhenTradeTypeRECHARGE()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "RECHARGE",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo)
			.ConfigureAwait(true);

		Assert.True(actual);
	}

	[Fact]
	public async Task TransferRecordsFound_WhenTradeTypeDRAW()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "DRAW",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.True(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeSETTLEMENT()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "SETTLEMENT",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeBETTING()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "BETTING",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeSYSTEM_REWARD()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "SYSTEM_REWARD",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeREWARD_ANCHOR_2()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "REWARD_ANCHOR_2",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeSEND_GIFT()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "SEND_GIFT",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeGAME_BETTING()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "GAME_BETTING",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeGAME_SETTLEMENT()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "GAME_SETTLEMENT",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeLOTTERY_BET_ORDER()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "LOTTERY_BET_ORDER",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeLOTTERY_NUMBER_SEET_ORDER()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "LOTTERY_NUMBER_SEET_ORDER",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task TransferRecordsNotFound_WhenTradeTypeLOTTERY_CANCELLATION_ORDER()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["list"] = new JsonArray
				{
					new JsonObject
					{
						["tradeType"] = "LOTTERY_CANCELLATION_ORDER",
					},
				},
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).ConfigureAwait(true);

		Assert.False(actual);
	}

	[Theory]
	[InlineData(400100)]
	[InlineData(400101)]
	[InlineData(400103)]
	[InlineData(400104)]
	[InlineData(400105)]
	[InlineData(400106)]
	[InlineData(400108)]
	[InlineData(400110)]
	[InlineData(400111)]
	[InlineData(400200)]
	[InlineData(400201)]
	[InlineData(400202)]
	[InlineData(400203)]
	[InlineData(400204)]
	[InlineData(400205)]
	public async Task ThrowErrorCodeException_WhenStatusCodeNot200(int code)
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var startTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var pageNum = 1;
		var pageSize = 200;
		var merchantPayNo = "01HZ1NJX774G30G455WPXFM353";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = TimeZoneInfo.ConvertTime(startTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var encoded = "78A8C3AA709E882C2570513843C8526417711F19E8429E7E3E0CB5169C37C98E8CAFA7DBE5D388D71612AA09CC40CA55EEEDF4F7F75E6D4992FFFD04636BAF239F5276CF17CB6952B57E920B1687EA137A00D20013303B9CF8BD0B7517A8C1DABFAB1E1D1399927A6BFA30898DDAB21F851ACD6838768F24A544812F5723D5225F901021A237A8F42108570AA845D15244506F126EBB2F512057FDFD7D48B20C538C213227D5DD2C4848B5A868F12CC2";
		var response = new JsonObject
		{
			["code"] = code,
			["data"] = null,
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/transfer/records").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["startTime"]!.GetValue<string>() == parameters["startTime"]!.GetValue<string>() &&
				x["endTime"]!.GetValue<string>() == parameters["endTime"]!.GetValue<string>() &&
				x["pageNum"]!.GetValue<int>() == parameters["pageNum"]!.GetValue<int>() &&
				x["pageSize"]!.GetValue<int>() == parameters["pageSize"]!.GetValue<int>() &&
				x["merchantPayNo"]!.AsArray()[0]!.GetValue<string>() == parameters["merchantPayNo"]!.AsArray()[0]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var exception = await Assert.ThrowsAsync<ErrorCodeException>(
			() => sut.TransferRecordsAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			startTime,
			endTime,
			pageNum,
			pageSize,
			merchantPayNo).AsTask())
			.ConfigureAwait(true);

		Assert.Equal(code, exception.Code);
	}
}
