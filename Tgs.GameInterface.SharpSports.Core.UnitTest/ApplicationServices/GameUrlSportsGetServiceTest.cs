using NSubstitute;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices.Models;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest.ApplicationServices;

public sealed class GameUrlSportsGetServiceTest
{
	[Fact]
	public async void Normal_Process()
	{
		var query = new GameUrlSportsQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			SecretKey: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029",
			IsMobile: false,
			Language: "zh-cn",
			Ip: "127.0.0.1");
		var response = "htts://xxx.xxx.xxx/xxx";
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.PlayerSportsLoginAsync(
				apiBaseUrl: Arg.Is(new Uri("https://api.yuansports1.com/")),
				merchantId: Arg.Is("c6a33e4e235e731b51d2f303199edeb4"),
				clientId: Arg.Is("2a954fedf9a203d646e1a7f3c32528df"),
				clientSecret: Arg.Is("1ff45c6ed7494b04940dc0bd9e0dca6b"),
				loginName: Arg.Is(query.PlayerCode),
				terType: Arg.Is(TerType.PC),
				language: Arg.Is("zh-cn"),
				ip: Arg.Is("127.0.0.1"))
			.Returns(ValueTask.FromResult(response));
		var sut = new GameUrlSportsGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query).ConfigureAwait(true);

		Assert.Equal("htts://xxx.xxx.xxx/xxx", actual.GameUrl);
	}
}
