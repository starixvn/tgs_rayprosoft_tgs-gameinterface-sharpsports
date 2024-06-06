using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;

using Tgs.GameInterface.SharpSports.Core.Models;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest.ApplicationServices;

public sealed class FundInGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundInQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029",
			OrderId: "01HZ106HC804392FD0Z04SMB8H",
			Amount: 4m);
		var json = new JsonObject
		{
			["tradeNo"] = "01HZ10EDSRTH4SC7SQ3VAA1SBA",
			["merchantPayNo"] = "01HZ106HC804392FD0Z04SMB8H",
			["balance"] = 8m
		};
		var response = new WalletDepositAndWithDrawResponse(
			TradeNo: json["tradeNo"]!.GetValue<string>(),
			MerchantPayNo: json["merchantPayNo"]!.GetValue<string>(),
			Balance: json["balance"]!.GetValue<decimal>(),
			Resp: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.WalletDepositAndWithDrawAsync(
				Arg.Is(query.ApiUrl),
				Arg.Is(query.MerchantId),
				Arg.Is(query.ClientId),
				Arg.Is(query.ClientSecret),
				Arg.Is(query.PlayerCode),
				Arg.Is(query.OrderId),
				Arg.Is(TradeType.WithDraw),
				Arg.Is(query.Amount))
			.Returns(ValueTask.FromResult(response));
		var sut = new FundInGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query).ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Equal("01HZ106HC804392FD0Z04SMB8H", actual.OrderId);
		Assert.Equal("01HZ10EDSRTH4SC7SQ3VAA1SBA", actual.ExternalOrderId);
		Assert.Equal(8m, actual.Balance);
		Assert.Equal(json.ToJsonString(), actual.Resp);
	}
}
