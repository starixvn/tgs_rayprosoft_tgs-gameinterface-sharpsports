using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest;

public sealed class GameApiClient_PlayerBalanceTest
{
	[Fact]
	public async void Normal_Process()
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
		};
		var encoded = "78A8C3AA709E882C2570513843C85264C9A55C38031EAE27592C4F9729ABDCCF";
		var response = new JsonObject
		{
			["code"] = 200,
			["data"] = new JsonObject
			{
				["balance"] = 16m,
			},
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/player/balance").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var actual = await sut.PlayerBalanceAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			loginName)
			.ConfigureAwait(true);

		Assert.Equal(16m, actual);
	}

	[Theory]
	[InlineData(400100)]
	[InlineData(400101)]
	[InlineData(400103)]
	[InlineData(400110)]
	[InlineData(400111)]
	[InlineData(400200)]
	[InlineData(400201)]
	[InlineData(400202)]
	[InlineData(400203)]
	[InlineData(400204)]
	[InlineData(400205)]
	public async void ThrowErrorCodeException_WhenStatusCodeNot200(int code)
	{
		var apiBaseUrl = new Uri("https://api.yuansports1.com/");
		var merchantId = "c6a33e4e235e731b51d2f303199edeb4";
		var clientId = "2a954fedf9a203d646e1a7f3c32528df";
		var clientSecret = "1ff45c6ed7494b04940dc0bd9e0dca6b";
		var loginName = "4ucmqplxb029";
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
		};
		var encoded = "78A8C3AA709E882C2570513843C85264C9A55C38031EAE27592C4F9729ABDCCF";
		var response = new JsonObject
		{
			["code"] = code,
			["data"] = null,
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/player/balance").ToString())
			.WithHeaders("client_id", clientId)
			.WithJsonContent(new JsonObject { ["data"] = encoded, ["merchantId"] = merchantId })
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeEncryptor = Substitute.For<IParameterEncryptor>();
		fakeEncryptor.Encrypt(
			Arg.Is<JsonObject>(x =>
				x["loginName"]!.GetValue<string>() == parameters["loginName"]!.GetValue<string>()),
			Arg.Is(clientSecret))
			.Returns(encoded);
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient, fakeEncryptor);

		var exception = await Assert.ThrowsAsync<ErrorCodeException>(
			() => sut.PlayerBalanceAsync(
				apiBaseUrl,
				merchantId,
				clientId,
				clientSecret,
				loginName).AsTask())
			.ConfigureAwait(true);

		Assert.Equal(code, exception.Code);
	}
}
