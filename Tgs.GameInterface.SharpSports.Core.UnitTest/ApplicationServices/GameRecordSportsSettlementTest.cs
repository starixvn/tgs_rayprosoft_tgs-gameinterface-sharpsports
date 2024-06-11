using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;

using Tgs.GameInterface.SharpSports.Core.Models;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest.ApplicationServices;

public sealed class GameRecordSportsSettlementTest
{
	[Fact]
	public async Task Win()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["gold"] = 10,
			["lang"] = "zh-tw",
			["merId"] = "c6a33e4e235e731b51d2f303199edeb4",
			["state"] = 2,
			["betNum"] = 1,
			["merName"] = "PG-CNY",
			["orderId"] = "QXT24060517201133513",
			["winGold"] = 19.3,
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
			["playerId"] = "506763ccb6d328049ccc7db080514a06",
			["realGold"] = 10,
			["remoteIp"] = "125.227.69.109",
			["walletId"] = 1,
			["loginName"] = "4ucmqplxb029",
			["parlayNum"] = "1",
			["betDTOList"] = new JsonArray {
				new JsonObject{
					["ratio"] = "11",
					["score"] = "",
					["gameId"] = "5857725171981148161420720904",
					["betItem"] = "UCAM穆尔西亚 +11",
					["ioRatio"] = "0.93",
					["session"] = "0",
					["awayTeam"] = "UCAM穆尔西亚",
					["betItem1"] = "CB Murcia +11",
					["gameDate"] = "2024-06-06",
					["gameTime"] = "02:30:00",
					["gameType"] = "BK",
					["homeTeam"] = "乌尼卡查马拉加",
					["playName"] = "让球",
					["playType"] = "R",
					["showType"] = "FT",
					["ratioType"] = "RC",
					["leagueName"] = "西班牙篮球甲级联赛",
					["teamSuffix"] = "",
					["betItemLang"] = "{\"en-us\":\"CB Murcia +11\",\"zh-tw\":\"UCAM穆爾西亞 +11\",\"zh-cn\":\"UCAM穆尔西亚 +11\",\"vi-vn\":\"CB Murcia +11\",\"th-th\":\"CB Murcia +11\"}",
					["gameTypeSon"] = null,
					["resultScore"] = "70 : 79",
					["awayLangTeam"] = "{\"en-us\":\"CB Murcia\",\"zh-tw\":\"UCAM穆爾西亞\",\"zh-cn\":\"UCAM穆尔西亚\",\"vi-vn\":\"CB Murcia\",\"th-th\":\"CB Murcia\"}",
					["homeLangTeam"] = "{\"en-us\":\"Malaga\",\"zh-tw\":\"烏尼卡查馬拉加\",\"zh-cn\":\"乌尼卡查马拉加\",\"vi-vn\":\"Unicaja Malaga\",\"th-th\":\"Malaga\"}",
					["playNameLang"] = "{\"en-us\":\"Handicap\",\"zh-tw\":\"讓球\",\"zh-cn\":\"让球\",\"vi-vn\":\"Cược chấp\",\"th-th\":\"แฮนดิแคป\"}",
					["leagueLangName"] = "{\"en-us\":\"Spain Liga ACB\",\"zh-tw\":\"西班牙籃球甲級聯賽\",\"zh-cn\":\"西班牙篮球甲级联赛\",\"vi-vn\":\"\",\"th-th\":\"Senegal Ligue 2\"}",
					["betResultDetail"] = "W",
					["leagueShortName"] = "西甲",
					["leagueShortLangName"] = "{\"en-us\":\"ESP Liga ACB\",\"zh-tw\":\"西甲\",\"zh-cn\":\"西甲\",\"vi-vn\":\"\",\"th-th\":\"Senegal Ligue 2\"}"
				}
			},
			["createDate"] = "2024-06-05 17:20:11",
			["playerName"] = "AliFardydon0Ty",
			["resultDate"] = "2024-06-06 04:18:58",
			["cashoutType"] = "0",
			["creditState"] = 1,
			["validAmount"] = 9.3,
			["resultDetail"] = null,
			["terminalType"] = "16",
			["realResultDate"] = "2024-06-06 04:18:58"
		};
		var responseItem = new SportOrderListResponseItem(
			LoginName: "4ucmqplxb029",
			OrderId: "QXT24060517201133513",
			State: SportOrderListState.Win,
			CashoutType: "0",
			CreateDate: new DateTimeOffset(DateTime.Parse("2024 -06-05 17:20:11"), TimeSpan.FromHours(+08)),
			ResultDate: new DateTimeOffset(DateTime.Parse("2024-06-06 04:18:58"), TimeSpan.FromHours(+08)),
			RealResultDate: new DateTimeOffset(DateTime.Parse("2024-06-06 04:18:58"), TimeSpan.FromHours(+08)),
			Gold: 10.0m,
			RealGold: 9.3m,
			WinGold: 19.3m,
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.ClientSecret),
				beginTime: Arg.Is(query.StartTime),
				endTime: Arg.Is(query.EndTime),
				orderState: Arg.Is(SportOrderListOrderState.Settled),
				timeType: Arg.Is(SportOrderListTimeType.ResultDate),
				pageNum: Arg.Is(1),
				pageSize: Arg.Is(200))
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("QXT24060517201133513", data.ExternalBetId);
		Assert.Equal("4ucmqplxb029", data.PlayerCode);
		Assert.Equal(SportOrderListState.Win, data.State);
		Assert.False(data.IsCashout);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-05 17:20:11"), TimeSpan.FromHours(8)), data.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-06 04:18:58"), TimeSpan.FromHours(8)), data.ExternalClosedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-06 04:18:58"), TimeSpan.FromHours(8)), data.ExternalPayoffOn);
		Assert.Equal(10m, data.Bet);
		Assert.Equal(9.3m, data.ValidBet);
		Assert.Equal(19.3m, data.Settle);
		Assert.Equal(9.3m, data.WinLoss);
		Assert.Equal(json.ToJsonString(), data.AdditionalData);
	}

	[Fact]
	public async Task Loss()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
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
		};
		var responseItem = new SportOrderListResponseItem(
			LoginName: "4um6m93suvtn",
			OrderId: "AIS24061114093035050",
			State: SportOrderListState.Loss,
			CashoutType: "0",
			CreateDate: new DateTimeOffset(DateTime.Parse("2024-06-11 14:09:30"), TimeSpan.FromHours(+08)),
			ResultDate: new DateTimeOffset(DateTime.Parse("2024-06-11 14:18:57"), TimeSpan.FromHours(+08)),
			RealResultDate: new DateTimeOffset(DateTime.Parse("2024-06-11 14:18:57"), TimeSpan.FromHours(+08)),
			Gold: 10.0m,
			RealGold: 10.0m,
			WinGold: 0.0m,
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.ClientSecret),
				beginTime: Arg.Is(query.StartTime),
				endTime: Arg.Is(query.EndTime),
				orderState: Arg.Is(SportOrderListOrderState.Settled),
				timeType: Arg.Is(SportOrderListTimeType.ResultDate),
				pageNum: Arg.Is(1),
				pageSize: Arg.Is(200))
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("AIS24061114093035050", data.ExternalBetId);
		Assert.Equal("4um6m93suvtn", data.PlayerCode);
		Assert.Equal(SportOrderListState.Loss, data.State);
		Assert.False(data.IsCashout);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 14:09:30"), TimeSpan.FromHours(8)), data.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 14:18:57"), TimeSpan.FromHours(8)), data.ExternalClosedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 14:18:57"), TimeSpan.FromHours(8)), data.ExternalPayoffOn);
		Assert.Equal(10.0m, data.Bet);
		Assert.Equal(10.0m, data.ValidBet);
		Assert.Equal(0.0m, data.Settle);
		Assert.Equal(-10.0m, data.WinLoss);
		Assert.Equal(json.ToJsonString(), data.AdditionalData);
	}

	[Fact]
	public async Task Draw()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
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
		};
		var responseItem = new SportOrderListResponseItem(
			LoginName: "4um6m93suvtn",
			OrderId: "AIS24061113291235047",
			State: SportOrderListState.Draw,
			CashoutType: "0",
			CreateDate: new DateTimeOffset(DateTime.Parse("2024-06-11 13:29:12"), TimeSpan.FromHours(+08)),
			ResultDate: new DateTimeOffset(DateTime.Parse("2024-06-11 13:34:52"), TimeSpan.FromHours(+08)),
			RealResultDate: new DateTimeOffset(DateTime.Parse("2024-06-11 13:34:52"), TimeSpan.FromHours(+08)),
			Gold: 10.0m,
			RealGold: 0.0m,
			WinGold: 10.0m,
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.ClientSecret),
				beginTime: Arg.Is(query.StartTime),
				endTime: Arg.Is(query.EndTime),
				orderState: Arg.Is(SportOrderListOrderState.Settled),
				timeType: Arg.Is(SportOrderListTimeType.ResultDate),
				pageNum: Arg.Is(1),
				pageSize: Arg.Is(200))
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("AIS24061113291235047", data.ExternalBetId);
		Assert.Equal("4um6m93suvtn", data.PlayerCode);
		Assert.Equal(SportOrderListState.Draw, data.State);
		Assert.False(data.IsCashout);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 13:29:12"), TimeSpan.FromHours(8)), data.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 13:34:52"), TimeSpan.FromHours(8)), data.ExternalClosedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-11 13:34:52"), TimeSpan.FromHours(8)), data.ExternalPayoffOn);
		Assert.Equal(10.0m, data.Bet);
		Assert.Equal(0.0m, data.ValidBet);
		Assert.Equal(10.0m, data.Settle);
		Assert.Equal(0.0m, data.WinLoss);
		Assert.Equal(json.ToJsonString(), data.AdditionalData);
	}

	[Fact]
	public async Task Cashout()
	{
		var query = new GameRecordSportsSettlementQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			StartTime: new DateTimeOffset(2024, 05, 29, 10, 50, 00, offset: TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 05, 29, 10, 50, 10, offset: TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 200);
		var json = new JsonObject
		{
			["gold"] = 10,
			["lang"] = "zh-tw",
			["merId"] = "c6a33e4e235e731b51d2f303199edeb4",
			["state"] = 3,
			["betNum"] = 1,
			["merName"] = "PG-CNY",
			["orderId"] = "QXT24060509054233411",
			["winGold"] = 9,
			["cashouts"] = new JsonObject
			{
				["stake"] = 10,
				["state"] = "Y",
				["initGold"] = 10,
				["wlAmount"] = -1,
				["cashoutId"] = "U1ee863c5-c6e3-47ab-b027-78b4df61b489",
				["returnAmount"] = 9,
				["placeDateTime"] = "2024-06-05 09:16:47"
			},
			["lineType"] = "2",
			["oddfType"] = "H",
			["playerId"] = "506763ccb6d328049ccc7db080514a06",
			["realGold"] = 10,
			["remoteIp"] = "125.227.69.109",
			["walletId"] = 1,
			["loginName"] = "4ucmqplxb029",
			["parlayNum"] = "1",
			["betDTOList"] = new JsonArray
			{
				new JsonObject{
					["ratio"] = "0",
					["score"] = "",
					["gameId"] = "5851809144314920961495891",
					["betItem"] = "拉鲁斯 0",
					["ioRatio"] = "1.09",
					["session"] = null,
					["awayTeam"] = "竞赛会",
					["betItem1"] = "CA Lanus 0",
					["gameDate"] = "2024-06-14",
					["gameTime"] = "08:15:00",
					["gameType"] = "FT",
					["homeTeam"] = "拉鲁斯",
					["playName"] = "让球",
					["playType"] = "R",
					["showType"] = "FU",
					["ratioType"] = "RH",
					["leagueName"] = "阿根廷甲级联赛",
					["teamSuffix"] = "",
					["betItemLang"] = "{\"en-us\":\"CA Lanus 0\",\"zh-tw\":\"拉魯斯 0\",\"zh-cn\":\"拉鲁斯 0\",\"vi-vn\":\"CA Lanus 0\",\"th-th\":\"CA Lanus 0\"}",
					["gameTypeSon"] = null,
					["resultScore"] = null,
					["awayLangTeam"] = "{\"en-us\":\"Racing Club Avellaneda\",\"zh-tw\":\"競賽會\",\"zh-cn\":\"竞赛会\",\"vi-vn\":\"Contest\",\"th-th\":\"Contest\"}",
					["homeLangTeam"] = "{\"en-us\":\"CA Lanus\",\"zh-tw\":\"拉魯斯\",\"zh-cn\":\"拉鲁斯\",\"vi-vn\":\"CA Lanus\",\"th-th\":\"CA Lanus\"}",
					["playNameLang"] = "{\"en-us\":\"Handicap\",\"zh-tw\":\"讓球\",\"zh-cn\":\"让球\",\"vi-vn\":\"Cược chấ\",\"th-th\":\"แฮนดิแคป \"}",
					["leagueLangName"] = "{\"en-us\":\"Argentine Primera División\",\"zh-tw\":\"阿根廷足球甲級聯賽\",\"zh-cn\":\"阿根廷甲级联赛\",\"vi-vn\":\"Argentina Superliga\",\"th-th\":\"Argentina Superliga\"}",
					["betResultDetail"] = "L",
					["leagueShortName"] = "阿甲",
					["leagueShortLangName"] = "{\"en-us\":\"ARG Primera División\",\"zh-tw\":\"阿甲\",\"zh-cn\":\"阿甲\",\"vi-vn\":\"Argentina Superliga\",\"th-th\":\"Argentina Superliga\"}"
				}
			},
			["createDate"] = "2024-06-05 09:05:42",
			["playerName"] = "AliFardydon0Ty",
			["resultDate"] = "2024-06-05 09:16:47",
			["cashoutType"] = "2",
			["creditState"] = 1,
			["validAmount"] = 1,
			["resultDetail"] = null,
			["terminalType"] = "16",
			["realResultDate"] = "2024-06-05 09:16:47"
		};
		var responseItem = new SportOrderListResponseItem(
			LoginName: "4ucmqplxb029",
			OrderId: "QXT24060509054233411",
			State: SportOrderListState.Loss,
			CashoutType: "2",
			CreateDate: new DateTimeOffset(DateTime.Parse("2024-06-05 09:05:42"), TimeSpan.FromHours(+08)),
			ResultDate: new DateTimeOffset(DateTime.Parse("2024-06-05 09:16:47"), TimeSpan.FromHours(+08)),
			RealResultDate: new DateTimeOffset(DateTime.Parse("2024-06-05 09:16:47"), TimeSpan.FromHours(+08)),
			Gold: 10.0m,
			RealGold: 1.0m,
			WinGold: 9.0m,
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.SportOrderListAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				merchantId: Arg.Is(query.MerchantId),
				clientId: Arg.Is(query.ClientId),
				clientSecret: Arg.Is(query.ClientSecret),
				beginTime: Arg.Is(query.StartTime),
				endTime: Arg.Is(query.EndTime),
				orderState: Arg.Is(SportOrderListOrderState.Settled),
				timeType: Arg.Is(SportOrderListTimeType.ResultDate),
				pageNum: Arg.Is(1),
				pageSize: Arg.Is(200))
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsSettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("QXT24060509054233411", data.ExternalBetId);
		Assert.Equal("4ucmqplxb029", data.PlayerCode);
		Assert.Equal(SportOrderListState.Loss, data.State);
		Assert.True(data.IsCashout);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-05 09:05:42"), TimeSpan.FromHours(8)), data.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-05 09:16:47"), TimeSpan.FromHours(8)), data.ExternalClosedOn);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-05 09:16:47"), TimeSpan.FromHours(8)), data.ExternalPayoffOn);
		Assert.Equal(10m, data.Bet);
		Assert.Equal(1.0m, data.ValidBet);
		Assert.Equal(9.0m, data.Settle);
		Assert.Equal(-1.0m, data.WinLoss);
		Assert.Equal(json.ToJsonString(), data.AdditionalData);
	}
}
