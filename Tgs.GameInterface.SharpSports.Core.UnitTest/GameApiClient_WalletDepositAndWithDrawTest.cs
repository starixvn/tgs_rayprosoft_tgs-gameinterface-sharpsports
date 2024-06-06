using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest;

public sealed class GameApiClient_WalletDepositAndWithDrawTest
{
	[Fact]
	public async void Normal_Process()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var merchantPayNo = "01HZ1P0A2SMNR1HZ3VPPGT541Y";
		var tradeType = TradeType.Deposit;
		var amount = 10m;
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["merchantPayNo"] = merchantPayNo,
			["tradeType"] = (byte)tradeType,
			["amount"] = amount,
		};
		var encoded = "78A8C3AA709E882C2570513843C852643A22544D7133A39E00F1886AF6FF0EFDBBD1C0C55783ACBC92D623CF4580EC09AC1A412EC31FF1172E9108B50E0F0E94A7D7CD485D86290BCE6E6CA89B541F0A92E3A8C671A13A73DE9336181418E1411CF527244EEA626A0A4BADCC6C0E0749";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["tradeNo"] = "01HZ1P35PAM7JC76ZFDW1QKYEC",
				["merchantPayNo"] = "01HZ1P3EQBB0K54M0RR1RH99S8",
				["balance"] = 20m,
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/wallet/depositAndWithDraw").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["merchantPayNo"]!.GetValue<string>() == parameters["merchantPayNo"]!.GetValue<string>() &&
				x["tradeType"]!.GetValue<byte>() == parameters["tradeType"]!.GetValue<byte>() &&
				x["amount"]!.GetValue<decimal>() == parameters["amount"]!.GetValue<decimal>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.WalletDepositAndWithDrawAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			merchantPayNo,
			tradeType,
			amount)
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Equal("01HZ1P35PAM7JC76ZFDW1QKYEC", actual.TradeNo);
		Assert.Equal("01HZ1P3EQBB0K54M0RR1RH99S8", actual.MerchantPayNo);
		Assert.Equal(20m, actual.Balance);
		Assert.Equal(response["data"]!.ToJsonString(), actual.Resp);
	}

	[Theory]
	[InlineData(400100)]
	[InlineData(400101)]
	[InlineData(400103)]
	[InlineData(400104)]
	[InlineData(400105)]
	[InlineData(400106)]
	[InlineData(400107)]
	[InlineData(400108)]
	[InlineData(400109)]
	[InlineData(400110)]
	[InlineData(400111)]
	[InlineData(400112)]
	[InlineData(400200)]
	[InlineData(400201)]
	[InlineData(400202)]
	[InlineData(400203)]
	[InlineData(400204)]
	[InlineData(400205)]
	[InlineData(500107)]
	[InlineData(500129)]
	public async void ThrowErrorCodeException_WhenStatusCodeNot200(int code)
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var merchantPayNo = "01HZ1P0A2SMNR1HZ3VPPGT541Y";
		var tradeType = TradeType.Deposit;
		var amount = 10m;
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["merchantPayNo"] = merchantPayNo,
			["tradeType"] = (byte)tradeType,
			["amount"] = amount,
		};
		var encoded = "78A8C3AA709E882C2570513843C852643A22544D7133A39E00F1886AF6FF0EFDBBD1C0C55783ACBC92D623CF4580EC09AC1A412EC31FF1172E9108B50E0F0E94A7D7CD485D86290BCE6E6CA89B541F0A92E3A8C671A13A73DE9336181418E1411CF527244EEA626A0A4BADCC6C0E0749";
		var response = new JsonObject
		{
			["code"] = code,
			["data"] = null,
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/wallet/depositAndWithDraw").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["merchantPayNo"]!.GetValue<string>() == parameters["merchantPayNo"]!.GetValue<string>() &&
				x["tradeType"]!.GetValue<byte>() == parameters["tradeType"]!.GetValue<byte>() &&
				x["amount"]!.GetValue<decimal>() == parameters["amount"]!.GetValue<decimal>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var exception = await Assert.ThrowsAsync<ErrorCodeException>(
			() => sut.WalletDepositAndWithDrawAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			merchantPayNo,
			tradeType,
			amount).AsTask())
			.ConfigureAwait(true);

		Assert.Equal(code, exception.Code);
	}
}
