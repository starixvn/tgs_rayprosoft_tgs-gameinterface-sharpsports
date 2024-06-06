using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest;

public sealed class GameApiClient_PlayerSportsLoginTest
{
	[Fact]
	public async void Normal_Process()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var terType = TerType.PC;
		var language = "zh-cn";
		var ip = "127.0.0.1";
		var encoded = "78A8C3AA709E882C2570513843C85264372A646B9F22C90A8DBF43470A8D4765C031D7E73A1EBEE1ADA93C900312958F3C376AEDB4E9C3FC6B103FB5507F41ECB00780790F39570D444055840C70E960";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["gameLink"] = "https://xxx.xxx.xxx/xxx",
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/player/sportsLogin").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(Arg.Any<JsonObject>(), clientSecret)
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.PlayerSportsLoginAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName,
			terType,
			language,
			ip)
			.ConfigureAwait(true);

		Assert.Equal("https://xxx.xxx.xxx/xxx", actual);
	}

	[Theory]
	[InlineData(400100)]
	[InlineData(400101)]
	[InlineData(400103)]
	[InlineData(400112)]
	[InlineData(400200)]
	[InlineData(400201)]
	[InlineData(400202)]
	[InlineData(400203)]
	[InlineData(400204)]
	[InlineData(400205)]
	[InlineData(400208)]
	[InlineData(400209)]
	public async void ThrowErrorCodeException_WhenStatusCodeNot200(int code)
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var terType = TerType.PC;
		var language = "zh-cn";
		var ip = "127.0.0.1";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["terType"] = (byte)terType,
			["language"] = language,
			["ip"] = ip,
		};
		var encoded = "78A8C3AA709E882C2570513843C85264372A646B9F22C90A8DBF43470A8D47651C81AF84DAB7E7F18968A23299FFDCA61016B7598C50318F8542C596C302B1986467AA37D39E7656F02B054F3EA081F3";
		var response = new JsonObject
		{
			["code"] = code,
			["data"] = null,
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/player/sportsLogin").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>() &&
				x["terType"]!.GetValue<byte>() == parameters["terType"]!.GetValue<byte>() &&
				x["language"]!.GetValue<string>() == parameters["language"]!.GetValue<string>() &&
				x["ip"]!.GetValue<string>() == parameters["ip"]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var exception = await Assert.ThrowsAsync<ErrorCodeException>(
			() => sut.PlayerSportsLoginAsync(
				apiBaseUrl,
				merchantId,
				clientId,
				clientSecret,
				loginName,
				terType,
				language,
				ip).AsTask())
			.ConfigureAwait(true);

		Assert.Equal(code, exception.Code);
	}
}
