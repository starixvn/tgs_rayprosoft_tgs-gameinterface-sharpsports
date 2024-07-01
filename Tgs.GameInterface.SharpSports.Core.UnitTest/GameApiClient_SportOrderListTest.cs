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
	public async Task Win()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var beginTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var orderState = SportOrderListOrderState.Settled;
		var timeType = SportOrderListTimeType.CreateDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["orderState"] = (byte)orderState,
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
						["gold"]= 10,
						["lang"]= "zh-tw",
						["merId"]= "c6a33e4e235e731b51d2f303199edeb4",
						["state"]= 2,
						["betNum"]= 1,
						["merName"]= "PG-CNY",
						["orderId"]= "QXT24060517201133513",
						["winGold"]= 19.3,
						["cashouts"]=new JsonObject {
							["stake"]= null,
							["state"]= null,
							["initGold"]= null,
							["wlAmount"]= null,
							["cashoutId"]= null,
							["returnAmount"]= null,
							["placeDateTime"]= null
						},
						["lineType"]= "2",
						["oddfType"]= "H",
						["playerId"]= "506763ccb6d328049ccc7db080514a06",
						["realGold"]= 10,
						["remoteIp"]= "125.227.69.109",
						["walletId"]= 1,
						["loginName"]= "4ucmqplxb029",
						["parlayNum"]= "1",
						["betDTOList"]=new JsonArray{
							new JsonObject
							{
								["ratio"]= "11",
								["score"]= "",
								["gameId"]= "5857725171981148161420720904",
								["betItem"]= "UCAM穆尔西亚 +11",
								["ioRatio"]= "0.93",
								["session"]= "0",
								["awayTeam"]= "UCAM穆尔西亚",
								["betItem1"]= "CB Murcia +11",
								["gameDate"]= "2024-06-06",
								["gameTime"]= "02:30:00",
								["gameType"]= "BK",
								["homeTeam"]= "乌尼卡查马拉加",
								["playName"]= "让球",
								["playType"]= "R",
								["showType"]= "FT",
								["ratioType"]= "RC",
								["leagueName"]= "西班牙篮球甲级联赛",
								["teamSuffix"]= "",
								["betItemLang"]= "{\"en-us\":\"CB Murcia +11\",\"zh-tw\":\"UCAM穆爾西亞 +11\",\"zh-cn\":\"UCAM穆尔西亚 +11\",\"vi-vn\":\"CB Murcia +11\",\"th-th\":\"CB Murcia +11\"}",
								["gameTypeSon"]= null,
								["resultScore"]= "70 : 79",
								["awayLangTeam"]= "{\"en-us\":\"CB Murcia\",\"zh-tw\":\"UCAM穆爾西亞\",\"zh-cn\":\"UCAM穆尔西亚\",\"vi-vn\":\"CB Murcia\",\"th-th\":\"CB Murcia\"}",
								["homeLangTeam"]= "{\"en-us\":\"Malaga\",\"zh-tw\":\"烏尼卡查馬拉加\",\"zh-cn\":\"乌尼卡查马拉加\",\"vi-vn\":\"Unicaja Malaga\",\"th-th\":\"Malaga\"}",
								["playNameLang"]= "{\"en-us\":\"Handicap\",\"zh-tw\":\"讓球\",\"zh-cn\":\"让球\",\"vi-vn\":\"Cược chấp\",\"th-th\":\"แฮนดิแคป\"}",
								["leagueLangName"]= "{\"en-us\":\"Spain Liga ACB\",\"zh-tw\":\"西班牙籃球甲級聯賽\",\"zh-cn\":\"西班牙篮球甲级联赛\",\"vi-vn\":\"\",\"th-th\":\"Senegal Ligue 2\"}",
								["betResultDetail"]= "W",
								["leagueShortName"]= "西甲",
								["leagueShortLangName"]= "{\"en-us\":\"ESP Liga ACB\",\"zh-tw\":\"西甲\",\"zh-cn\":\"西甲\",\"vi-vn\":\"\",\"th-th\":\"Senegal Ligue 2\"}"
							}
						},
							["createDate"]= "2024-06-05 17:20:11",
							["playerName"]= "AliFardydon0Ty",
							["resultDate"]= "2024-06-06 04:18:58",
							["cashoutType"]= "0",
							["creditState"]= 1,
							["validAmount"]= 9.3,
							["resultDetail"]= null,
							["terminalType"]= "16",
							["realResultDate"]= "2024-06-06 04:18:58"
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
			orderState,
			timeType,
			pageNum,
			pageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("4ucmqplxb029", data.LoginName);
		Assert.Equal("QXT24060517201133513", data.OrderId);
		Assert.Equal(SportOrderListState.Win, data.State);
		Assert.Equal("0", data.CashoutType);
		Assert.Equal("2", data.LineType);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-05 17:20:11"), TimeSpan.FromHours(8)), data.CreateDate);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-06 04:18:58"), TimeSpan.FromHours(8)), data.ResultDate);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-06 04:18:58"), TimeSpan.FromHours(8)), data.RealResultDate);
		Assert.Equal(10.0m, data.Gold);
		Assert.Equal(9.3m, data.RealGold);
		Assert.Equal(19.3m, data.WinGold);
		Assert.Equal(response["data"]!["orderDTO"]!.AsArray()[0]!.ToJsonString(), data.AdditionalData);
	}

	[Fact]
	public async Task Loss()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var beginTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var orderState = SportOrderListOrderState.Settled;
		var timeType = SportOrderListTimeType.CreateDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["orderState"] = (byte)orderState,
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
						["gold"] = 10,
						["lang"] = "zh-cn",
						["merId"] = "c6a33e4e235e731b51d2f303199edeb4",
						["state"] = 3,
						["betNum"] = 1,
						["merName"] = "PG-CNY",
						["orderId"] = "AIS24061114093035050",
						["winGold"] = 0,
						["cashouts"] = new JsonObject
						{
							["stake"] = null,
							["state"] = null,
							["initGold"] = null,
							["wlAmount"] = null,
							["cashoutId"] = null,
							["returnAmount"] = null,
							["placeDateTime"] = null
						},
						["lineType"] = "2",
						["oddfType"] = "H",
						["playerId"] = "2864cf4f761cde5a6e3413b49efa2d0d",
						["realGold"] = 10,
						["remoteIp"] = "125.227.69.109",
						["walletId"] = 1,
						["loginName"] = "4um6m93suvtn",
						["parlayNum"] = "1",
						["betDTOList"] = new JsonArray {
							new JsonObject
							{
								["ratio"] = "0.75",
								["score"] = "0:1",
								["gameId"] = "5882463674753146881757607540",
								["betItem"] = "利物浦 (Wboy) -0.5/1",
								["ioRatio"] = "0.71",
								["session"] = null,
								["awayTeam"] = "Man City (nikkitta)",
								["betItem1"] = "Liverpool (Wboy) -0.5/1",
								["gameDate"] = "2024-06-11",
								["gameTime"] = "14:08:00",
								["gameType"] = "FT",
								["homeTeam"] = "利物浦 (Wboy)",
								["playName"] = "滚球:让球",
								["playType"] = "RE",
								["showType"] = "RB",
								["ratioType"] = "REH",
								["leagueName"] = "E-Soccer 足球电竞之战 - 8 分钟赛事",
								["teamSuffix"] = "",
								["betItemLang"] = "{\"en-us\":\"Liverpool (Wboy) -0.5/1\",\"zh-tw\":\"Liverpool (Wboy) -0.5/1\",\"zh-cn\":\"利物浦 (Wboy) -0.5/1\",\"vi-vn\":\"Liverpool (Wboy) -0.5/1\",\"th-th\":\"Liverpool (Wboy) -0.5/1\"}",
								["gameTypeSon"] = null,
								["resultScore"] = "0 : 2",
								["awayLangTeam"] = "{\"en-us\":\"Man City (nikkitta)\",\"zh-tw\":\"Man City (nikkitta)\",\"zh-cn\":\"Man City (nikkitta)\",\"th-th\":\"แมนเชสเตอร์ซิตี (nikkitta)\"}",
								["homeLangTeam"] = "{\"en-us\":\"Liverpool (Wboy)\",\"zh-tw\":\"Liverpool (Wboy)\",\"zh-cn\":\"利物浦 (Wboy)\",\"vi-vn\":\"Liverpool (Wboy)\",\"th-th\":\"Liverpool (Wboy)\"}",
								["playNameLang"] = "{\"en-us\":\"Handicap\",\"zh-tw\":\"滾球:讓球\",\"zh-cn\":\"滚球:让球\",\"vi-vn\":\"Bóng lăn Chấp bóng\",\"th-th\":\"แฮนดิแคปสด\"}",
								["leagueLangName"] = "{\"en-us\":\"E-Soccer Battle - 8 min\",\"zh-tw\":\"E-Soccer 足球電競之戰 - 8 分鐘賽事\",\"zh-cn\":\"E-Soccer 足球电竞之战 - 8 分钟赛事\",\"th-th\":\"E-Soccer แข่งขัน - 8 นาที\"}",
								["betResultDetail"] = "L",
								["leagueShortName"] = "E-Soccer 足球电竞之战 - 8 分钟赛事",
								["leagueShortLangName"] = "{\"en-us\":\"E-Soccer Battle - 8 min\",\"zh-tw\":\"E-Soccer 足球電競之戰 - 8 分鐘賽事\",\"zh-cn\":\"E-Soccer 足球电竞之战 - 8 分钟赛事\",\"th-th\":\"E-Soccer แข่งขัน - 8 นาที\"}"
							}
						},
						["createDate"] = "2024-06-11 14:09:30",
						["playerName"] = "侯赛因·图克曼t6i",
						["resultDate"] = "2024-06-11 14:18:57",
						["cashoutType"] = "0",
						["creditState"] = 1,
						["validAmount"] = 10,
						["resultDetail"] = null,
						["terminalType"] = "17",
						["realResultDate"] = "2024-06-11 14:18:57"
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
			orderState,
			timeType,
			pageNum,
			pageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("4um6m93suvtn", data.LoginName);
		Assert.Equal("AIS24061114093035050", data.OrderId);
		Assert.Equal(SportOrderListState.Loss, data.State);
		Assert.Equal("0", data.CashoutType);
		Assert.Equal("2", data.LineType);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 14:09:30"), TimeSpan.FromHours(8)), data.CreateDate);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 14:18:57"), TimeSpan.FromHours(8)), data.ResultDate);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 14:18:57"), TimeSpan.FromHours(8)), data.RealResultDate);
		Assert.Equal(10.0m, data.Gold);
		Assert.Equal(10.0m, data.RealGold);
		Assert.Equal(0.0m, data.WinGold);
		Assert.Equal(response["data"]!["orderDTO"]!.AsArray()[0]!.ToJsonString(), data.AdditionalData);
	}

	[Fact]
	public async Task Draw()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var beginTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var orderState = SportOrderListOrderState.Settled;
		var timeType = SportOrderListTimeType.CreateDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["orderState"] = (byte)orderState,
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
						["gold"] = 10,
						["lang"] = "zh-cn",
						["merId"] = "c6a33e4e235e731b51d2f303199edeb4",
						["state"] = 4,
						["betNum"] = 1,
						["merName"] = "PG-CNY",
						["orderId"] = "AIS24061113291235047",
						["winGold"] = 10,
						["cashouts"] = new JsonObject
						{
							["stake"] = null,
							["state"] = null,
							["initGold"] = null,
							["wlAmount"] = null,
							["cashoutId"] = null,
							["returnAmount"] = null,
							["placeDateTime"] = null
						},
						["lineType"] = "2",
						["oddfType"] = "H",
						["playerId"] = "2864cf4f761cde5a6e3413b49efa2d0d",
						["realGold"] = 10,
						["remoteIp"] = "125.227.69.109",
						["walletId"] = 1,
						["loginName"] = "4um6m93suvtn",
						["parlayNum"] = "1",
						["betDTOList"] = new JsonArray {
							new JsonObject{
								["ratio"] = "0",
								["score"] = "0:0",
								["gameId"] = "5882327728719790081495891",
								["betItem"] = "England (Lowheels) 0",
								["ioRatio"] = "0.81",
								["session"] = null,
								["awayTeam"] = "England (Lowheels)",
								["betItem1"] = "England (Lowheels) 0",
								["gameDate"] = "2024-06-11",
								["gameTime"] = "13:24:00",
								["gameType"] = "FT",
								["homeTeam"] = "Germany (Jekunam)",
								["playName"] = "滚球:让球",
								["playType"] = "RE",
								["showType"] = "RB",
								["ratioType"] = "REC",
								["leagueName"] = "E-Soccer 足球电竞之战 - 8 分钟赛事",
								["teamSuffix"] = "",
								["betItemLang"] = "{\"en-us\":\"England (Lowheels) 0\",\"zh-tw\":\"England (Lowheels) 0\",\"zh-cn\":\"England (Lowheels) 0\"}",
								["gameTypeSon"] = null,
								["resultScore"] = "0 : 0",
								["awayLangTeam"] = "{\"en-us\":\"England (Lowheels)\",\"zh-tw\":\"England (Lowheels)\",\"zh-cn\":\"England (Lowheels)\"}",
								["homeLangTeam"] = "{\"en-us\":\"Germany (Jekunam)\",\"zh-tw\":\"Germany (Jekunam)\",\"zh-cn\":\"Germany (Jekunam)\"}",
								["playNameLang"] = "{\"en-us\":\"Handicap\",\"zh-tw\":\"滾球:讓球\",\"zh-cn\":\"滚球:让球\"}",
								["leagueLangName"] = "{\"en-us\":\"E-Soccer Battle - 8 min\",\"zh-tw\":\"E-Soccer 足球電競之戰 - 8 分鐘賽事\",\"zh-cn\":\"E-Soccer 足球电竞之战 - 8 分钟赛事\"}",
								["betResultDetail"] = "P",
								["leagueShortName"] = "E-Soccer 足球电竞之战 - 8 分钟赛事",
								["leagueShortLangName"] = "{\"en-us\":\"E-Soccer Battle - 8 min\",\"zh-tw\":\"E-Soccer 足球電競之戰 - 8 分鐘賽事\",\"zh-cn\":\"E-Soccer 足球电竞之战 - 8 分钟赛事\"}"
							}
						},
			["createDate"] = "2024-06-11 13:29:12",
			["playerName"] = "侯赛因·图克曼t6i",
			["resultDate"] = "2024-06-11 13:34:52",
			["cashoutType"] = "0",
			["creditState"] = 1,
			["validAmount"] = 0,
			["resultDetail"] = null,
			["terminalType"] = "17",
			["realResultDate"] = "2024-06-11 13:34:52"
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
			orderState,
			timeType,
			pageNum,
			pageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("4um6m93suvtn", data.LoginName);
		Assert.Equal("AIS24061113291235047", data.OrderId);
		Assert.Equal(SportOrderListState.Draw, data.State);
		Assert.Equal("0", data.CashoutType);
		Assert.Equal("2", data.LineType);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 13:29:12"), TimeSpan.FromHours(8)), data.CreateDate);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 13:34:52"), TimeSpan.FromHours(8)), data.ResultDate);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 13:34:52"), TimeSpan.FromHours(8)), data.RealResultDate);
		Assert.Equal(10.0m, data.Gold);
		Assert.Equal(0.0m, data.RealGold);
		Assert.Equal(10.0m, data.WinGold);
		Assert.Equal(response["data"]!["orderDTO"]!.AsArray()[0]!.ToJsonString(), data.AdditionalData);
	}

	[Fact]
	public async Task Running()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var beginTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var orderState = SportOrderListOrderState.Unsettled;
		var timeType = SportOrderListTimeType.CreateDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["orderState"] = (byte)orderState,
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
						["merId"]= "c6a33e4e235e731b51d2f303199edeb4",
						["merName"]= "PG-CNY",
						["playerId"]= "506763ccb6d328049ccc7db080514a06",
						["loginName"]= "4ucmqplxb029",
						["playerName"]= "AliFardydon0Ty",
						["remoteIp"]= "125.227.69.109",
						["orderId"]= "QXT24060509070433422",
						["parlayNum"]= "3",
						["gold"]= 10,
						["realGold"]= 10,
						["winGold"]= null,
						["state"]= 1,
						["creditState"]= 0,
						["realResultDate"]= null,
						["resultDate"]= "",
						["resultDetail"]= null,
						["createDate"]= "2024-06-05 09:07:04",
						["terminalType"]= "16",
						["oddfType"]= "E",
						["betDTOList"]= new JsonArray{
							new JsonObject{
								["showType"]= "FU",
								["gameType"]= "FT",
								["playType"]= "R",
								["ratioType"]= "RH",
								["gameDate"]= "2024-06-14",
								["gameTime"]= "08:15:00",
								["awayTeam"]= "竞赛会",
								["homeTeam"]= "拉鲁斯",
								["leagueName"]= "阿根廷甲级联赛",
								["leagueShortName"]= "阿甲",
								["score"]= null,
								["ioRatio"]= "2.09",
								["ratio"]= "0",
								["resultScore"]= null,
								["betItem"]= "拉鲁斯 0",
								["betResultDetail"]= null,
								["session"]= null,
								["teamSuffix"]= "",
								["awayLangTeam"]= "{\"en-us\":\"Racing Club Avellaneda\",\"zh-tw\":\"競賽會\",\"zh-cn\":\"竞赛会\",\"vi-vn\":\"Contest\",\"th-th\":\"Contest\"}",
								["homeLangTeam"]= "{\"en-us\":\"CA Lanus\",\"zh-tw\":\"拉魯斯\",\"zh-cn\":\"拉鲁斯\",\"vi-vn\":\"CA Lanus\",\"th-th\":\"CA Lanus\"}",
								["leagueLangName"]= "{\"en-us\":\"Argentine Primera División\",\"zh-tw\":\"阿根廷足球甲級聯賽\",\"zh-cn\":\"阿根廷甲级联赛\",\"vi-vn\":\"Argentina Superliga\",\"th-th\":\"Argentina Superliga\"}",
								["leagueShortLangName"]= "{\"en-us\":\"ARG Primera División\",\"zh-tw\":\"阿甲\",\"zh-cn\":\"阿甲\",\"vi-vn\":\"Argentina Superliga\",\"th-th\":\"Argentina Superliga\"}",
								["betItem1"]= "CA Lanus 0",
								["playNameLang"]= "{\"en-us\":\"Handicap\",\"zh-tw\":\"讓球\",\"zh-cn\":\"让球\",\"vi-vn\":\"Cược chấ\",\"th-th\":\"แฮนดิแคป \"}",
								["playName"]= "让球",
								["betItemLang"]= "{\"en-us\":\"CA Lanus 0\",\"zh-tw\":\"拉魯斯 0\",\"zh-cn\":\"拉鲁斯 0\",\"vi-vn\":\"CA Lanus 0\",\"th-th\":\"CA Lanus 0\"}",
								["gameId"]= "5851809144314920961495891",
								["gameTypeSon"]= null},
							new JsonObject{
								["showType"]= "FU",
								["gameType"]= "FT",
								["playType"]= "R",
								["ratioType"]= "RH",
								["gameDate"]= "2024-06-14",
								["gameTime"]= "19:35:00",
								["awayTeam"]= "山东泰山",
								["homeTeam"]= "梅州客家",
								["leagueName"]= "中国足球超级联赛",
								["leagueShortName"]= "中超",
								["score"]= null,
								["ioRatio"]= "1.83",
								["ratio"]= "1",
								["resultScore"]= null,
								["betItem"]= "梅州客家 +1",
								["betResultDetail"]= null,
								["session"]= null,
								["teamSuffix"]= "",
								["awayLangTeam"]= "{\"en-us\":\"Shandong Taishan \",\"zh-tw\":\"山東泰山\",\"zh-cn\":\"山东泰山\",\"vi-vn\":\"Shandong Taishan\",\"th-th\":\"Shandong  Taishan\"}",
								["homeLangTeam"]= "{\"en-us\":\"Meizhou Hakka FC\",\"zh-tw\":\"梅州客家\",\"zh-cn\":\"梅州客家\",\"vi-vn\":\"Meizhou Hakka\",\"th-th\":\"Meizhou Hakka FC\"}",
								["leagueLangName"]= "{\"en-us\":\"Chinese Super League\",\"zh-tw\":\"中國足球超級聯賽\",\"zh-cn\":\"中国足球超级联赛\",\"vi-vn\":\"\",\"th-th\":\"Chinese Super League\"}",
								["leagueShortLangName"]= "{\"en-us\":\"CHN CSL\",\"zh-tw\":\"中超\",\"zh-cn\":\"中超\",\"vi-vn\":\"\",\"th-th\":\"Chinese Super League\"}",
								["betItem1"]= "Meizhou Hakka FC +1",
								["playNameLang"]= "{\"en-us\":\"Handicap\",\"zh-tw\":\"讓球\",\"zh-cn\":\"让球\",\"vi-vn\":\"Cược chấ\",\"th-th\":\"แฮนดิแคป \"}",
								["playName"]= "让球",
								["betItemLang"]= "{\"en-us\":\"Meizhou Hakka FC +1\",\"zh-tw\":\"梅州客家 +1\",\"zh-cn\":\"梅州客家 +1\",\"vi-vn\":\"Meizhou Hakka +1\",\"th-th\":\"Meizhou Hakka FC +1\"}",
								["gameId"]= "5854069197042933761495892",
								["gameTypeSon"]= null
							},
							new JsonObject{
								["showType"]= "FU",
								["gameType"]= "FT",
								["playType"]= "R",
								["ratioType"]= "RH",
								["gameDate"]= "2024-06-14",
								["gameTime"]= "20:00:00",
								["awayTeam"]= "上海海港",
								["homeTeam"]= "天津津门虎",
								["leagueName"]= "中国足球超级联赛",
								["leagueShortName"]= "中超",
								["score"]= null,
								["ioRatio"]= "1.88",
								["ratio"]= "0.75",
								["resultScore"]= null,
								["betItem"]= "天津津门虎 +0.5/1",
								["betResultDetail"]= null,
								["session"]= null,
								["teamSuffix"]= "",
								["awayLangTeam"]= "{\"en-us\":\"Shanghai Seaport\",\"zh-tw\":\"上海海港\",\"zh-cn\":\"上海海港\",\"vi-vn\":\"Cảng Thượng Hải\",\"th-th\":\"Shanghai Seaport\"}",
								["homeLangTeam"]= "{\"en-us\":\"Tianjin Jinmen Tiger\",\"zh-tw\":\"天津津門虎\",\"zh-cn\":\"天津津门虎\",\"vi-vn\":\"Thiên Tân Jinmen Tiger\",\"th-th\":\"Tianjin Jinmen Tiger\"}",
								["leagueLangName"]= "{\"en-us\":\"Chinese Super League\",\"zh-tw\":\"中國足球超級聯賽\",\"zh-cn\":\"中国足球超级联赛\",\"vi-vn\":\"\",\"th-th\":\"Chinese Super League\"}",
								["leagueShortLangName"]= "{\"en-us\":\"CHN CSL\",\"zh-tw\":\"中超\",\"zh-cn\":\"中超\",\"vi-vn\":\"\",\"th-th\":\"Chinese Super League\"}",
								["betItem1"]= "Tianjin Jinmen Tiger +0.5/1",
								["playNameLang"]= "{\"en-us\":\"Handicap\",\"zh-tw\":\"讓球\",\"zh-cn\":\"让球\",\"vi-vn\":\"Cược chấ\",\"th-th\":\"แฮนดิแคป \"}",
								["playName"]= "让球",
								["betItemLang"]= "{\"en-us\":\"Tianjin Jinmen Tiger +0.5/1\",\"zh-tw\":\"天津津門虎 +0.5/1\",\"zh-cn\":\"天津津门虎 +0.5/1\",\"vi-vn\":\"Thiên Tân Jinmen Tiger +0.5/1\",\"th-th\":\"Tianjin Jinmen Tiger +0.5/1\"}",
								["gameId"]= "5854069649314734081614461785",
								["gameTypeSon"]= null
							}
						},
						["lineType"]= "1",
						["cashoutType"]= "0",
						["cashouts"]=new JsonObject {
							["cashoutId"]= null,
							["stake"]= null,
							["initGold"]= null,
							["returnAmount"]= null,
							["wlAmount"]= null,
							["placeDateTime"]= null,
							["state"]= null
						},
						["betNum"]= 1,
						["lang"]= "zh-tw",
						["walletId"]= 1,
						["validAmount"]= null
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
			apiBaseUrl: apiBaseUrl,
			merchantId: merchantId,
			clientId: clientId,
			clientSecret: clientSecret,
			beginTime: beginTime,
			endTime: endTime,
			orderState: orderState,
			timeType: timeType,
			pageNum: pageNum,
			pageSize: pageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("4ucmqplxb029", data.LoginName);
		Assert.Equal("QXT24060509070433422", data.OrderId);
		Assert.Equal(SportOrderListState.Running, data.State);
		Assert.Equal("0", data.CashoutType);
		Assert.Equal("1", data.LineType);
		Assert.Equal(new DateTimeOffset(2024, 06, 05, 09, 07, 04, TimeSpan.FromHours(8)), data.CreateDate);
		Assert.Equal(DateTime.UnixEpoch, data.ResultDate);
		Assert.Equal(DateTime.UnixEpoch, data.RealResultDate);
		Assert.Equal(10m, data.Gold);
		Assert.Equal(10m, data.RealGold);
		Assert.Equal(0.0m, data.WinGold);
		Assert.Equal(response["data"]!["orderDTO"]!.AsArray()[0]!.ToJsonString(), data.AdditionalData);
	}

	[Fact]
	public async Task Cashout()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var beginTime = new DateTimeOffset(2024, 05, 30, 10, 00, 00, TimeSpan.FromHours(8));
		var endTime = new DateTimeOffset(2024, 05, 30, 10, 10, 00, TimeSpan.FromHours(8));
		var orderState = SportOrderListOrderState.Settled;
		var timeType = SportOrderListTimeType.RealResultDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["orderState"] = (byte)orderState,
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
						["gold"]= 10,
						["lang"]= "zh-tw",
						["merId"]= "c6a33e4e235e731b51d2f303199edeb4",
						["state"]= 3,
						["betNum"]= 1,
						["merName"]= "PG-CNY",
						["orderId"]= "QXT24060509054233411",
						["winGold"]= 9,
						["cashouts"]= new JsonObject{
							["stake"]= 10,
							["state"]= "Y",
							["initGold"]= 10,
							["wlAmount"]= -1,
							["cashoutId"]= "U1ee863c5-c6e3-47ab-b027-78b4df61b489",
							["returnAmount"]= 9,
							["placeDateTime"]= "2024-06-05 09:16:47"
						},
						["lineType"]= "2",
						["oddfType"]= "H",
						["playerId"]= "506763ccb6d328049ccc7db080514a06",
						["realGold"]= 10,
						["remoteIp"]= "125.227.69.109",
						["walletId"]= 1,
						["loginName"]= "4ucmqplxb029",
						["parlayNum"]= "1",
						["betDTOList"]= new JsonArray{
							new JsonObject{
								["ratio"]= "0",
								["score"]= "",
								["gameId"]= "5851809144314920961495891",
								["betItem"]= "拉鲁斯 0",
								["ioRatio"]= "1.09",
								["session"]= null,
								["awayTeam"]= "竞赛会",
								["betItem1"]= "CA Lanus 0",
								["gameDate"]= "2024-06-14",
								["gameTime"]= "08:15:00",
								["gameType"]= "FT",
								["homeTeam"]= "拉鲁斯",
								["playName"]= "让球",
								["playType"]= "R",
								["showType"]= "FU",
								["ratioType"]= "RH",
								["leagueName"]= "阿根廷甲级联赛",
								["teamSuffix"]= "",
								["betItemLang"]= "{\"en-us\":\"CA Lanus 0\",\"zh-tw\":\"拉魯斯 0\",\"zh-cn\":\"拉鲁斯 0\",\"vi-vn\":\"CA Lanus 0\",\"th-th\":\"CA Lanus 0\"}",
								["gameTypeSon"]= null,
								["resultScore"]= null,
								["awayLangTeam"]= "{\"en-us\":\"Racing Club Avellaneda\",\"zh-tw\":\"競賽會\",\"zh-cn\":\"竞赛会\",\"vi-vn\":\"Contest\",\"th-th\":\"Contest\"}",
								["homeLangTeam"]= "{\"en-us\":\"CA Lanus\",\"zh-tw\":\"拉魯斯\",\"zh-cn\":\"拉鲁斯\",\"vi-vn\":\"CA Lanus\",\"th-th\":\"CA Lanus\"}",
								["playNameLang"]= "{\"en-us\":\"Handicap\",\"zh-tw\":\"讓球\",\"zh-cn\":\"让球\",\"vi-vn\":\"Cược chấ\",\"th-th\":\"แฮนดิแคป \"}",
								["leagueLangName"]= "{\"en-us\":\"Argentine Primera División\",\"zh-tw\":\"阿根廷足球甲級聯賽\",\"zh-cn\":\"阿根廷甲级联赛\",\"vi-vn\":\"Argentina Superliga\",\"th-th\":\"Argentina Superliga\"}",
								["betResultDetail"]= "L",
								["leagueShortName"]= "阿甲",
								["leagueShortLangName"]= "{\"en-us\":\"ARG Primera División\",\"zh-tw\":\"阿甲\",\"zh-cn\":\"阿甲\",\"vi-vn\":\"Argentina Superliga\",\"th-th\":\"Argentina Superliga\"}"
							}
						},
						["createDate"]= "2024-06-05 09:05:42",
						["playerName"]= "AliFardydon0Ty",
						["resultDate"]= "2024-06-05 09:16:47",
						["cashoutType"]= "2",
						["creditState"]= 1,
						["validAmount"]= 1,
						["resultDetail"]= null,
						["terminalType"]= "16",
						["realResultDate"]= "2024-06-05 09:16:47"
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
			orderState,
			timeType,
			pageNum,
			pageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("4ucmqplxb029", data.LoginName);
		Assert.Equal("QXT24060509054233411", data.OrderId);
		Assert.Equal(SportOrderListState.Loss, data.State);
		Assert.Equal("2", data.CashoutType);
		Assert.Equal("2", data.LineType);
		Assert.Equal(new DateTimeOffset(2024, 06, 05, 09, 05, 42, TimeSpan.FromHours(8)), data.CreateDate);
		Assert.Equal(new DateTimeOffset(2024, 06, 05, 09, 16, 47, TimeSpan.FromHours(8)), data.ResultDate);
		Assert.Equal(new DateTimeOffset(2024, 06, 05, 09, 16, 47, TimeSpan.FromHours(8)), data.RealResultDate);
		Assert.Equal(10m, data.Gold);
		Assert.Equal(1.0m, data.RealGold);
		Assert.Equal(9.0m, data.WinGold);
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
		var orderState = SportOrderListOrderState.Settled;
		var timeType = SportOrderListTimeType.CreateDate;
		var pageNum = 1;
		var pageSize = 200;
		var parameters = new JsonObject
		{
			["beginTime"] = TimeZoneInfo.ConvertTime(beginTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["endTime"] = TimeZoneInfo.ConvertTime(endTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH:mm:ss"),
			["orderState"] = (byte)orderState,
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
			orderState,
			timeType,
			pageNum,
			pageSize).ToArrayAsync().AsTask())
			.ConfigureAwait(true);

		Assert.Equal(code, exception.Code);
	}
}
