using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;

using Tgs.GameInterface.SharpSports.Core.Models;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest.ApplicationServices;

public sealed class GameRecordSportsUnsettlementTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new GameRecordSportsUnsettlementQuery(
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
		var responseItem = new SportOrderListResponseItem(
			LoginName: "4ucmqplxb029",
			OrderId: "QXT24060509004433396",
			State: 1,
			CashoutType: "0",
			CreateDate: new DateTimeOffset(2024, 06, 05, 09, 00, 44, TimeSpan.FromHours(+8)),
			ResultDate: DateTimeOffset.UnixEpoch,
			RealResultDate: DateTimeOffset.UnixEpoch,
			Gold: 10.0m,
			RealGold: 10.0m,
			WinGold: 0.0m,
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient.SportOrderListAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			merchantId: Arg.Is(query.MerchantId),
			clientId: Arg.Is(query.ClientId),
			clientSecret: Arg.Is(query.ClientSecret),
			beginTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			orderState: Arg.Is(SportOrderListOrderState.Unsettled),
			timeType: Arg.Is(SportOrderListTimeType.CreateDate),
			pageNum: Arg.Is(1),
			pageSize: Arg.Is(200),
			Arg.Any<CancellationToken>())
			.Returns(new[] { responseItem }.ToAsyncEnumerable());
		var sut = new GameRecordSportsUnsettlement(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Single(actual);
		var data = actual[0];
		Assert.Equal("QXT24060509004433396", data.ExternalBetId);
		Assert.Equal("4ucmqplxb029", data.PlayerCode);
		Assert.False(data.IsClosed);
		Assert.False(data.IsCancel);
		Assert.False(data.IsRefund);
		Assert.False(data.IsVoid);
		Assert.False(data.IsCashout);
		Assert.Equal(new DateTimeOffset(DateTime.Parse("2024-06-05 09:00:44"), TimeSpan.FromHours(8)), data.ExternalCreatedOn);
		Assert.Equal(DateTime.UnixEpoch, data.ExternalClosedOn);
		Assert.Equal(DateTime.UnixEpoch, data.ExternalPayoffOn);
		Assert.Equal(10m, data.Bet);
		Assert.Equal(10m, data.ValidBet);
		Assert.Equal(0.0m, data.Settle);
		Assert.Equal(0.0m, data.WinLoss);
		Assert.Equal(json.ToJsonString(), data.AdditionalData);
	}
}
