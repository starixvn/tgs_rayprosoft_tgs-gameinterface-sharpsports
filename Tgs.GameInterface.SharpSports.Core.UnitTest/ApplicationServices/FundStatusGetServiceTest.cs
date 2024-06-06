using NSubstitute;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest.ApplicationServices;

public sealed class FundStatusGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundStatusQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029",
			OrderOn: DateTimeOffset.UtcNow,
			OrderId: "01HZ12X6SFBJ8RV7ZAT6WXPNV4");
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TransferRecordsAsync(
				Arg.Is(query.ApiUrl),
				Arg.Is(query.MerchantId),
				Arg.Is(query.ClientId),
				Arg.Is(query.ClientSecret),
				Arg.Is(query.PlayerCode),
				Arg.Is(query.OrderOn.AddHours(-6)),
				Arg.Is(query.OrderOn.Add(new(hours: 5, minutes: 59, seconds: 59))),
				Arg.Is(1),
				Arg.Is(200),
				Arg.Is(query.OrderId))
			.Returns(ValueTask.FromResult(true));
		var sut = new FundStatusGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query).ConfigureAwait(true);

		Assert.True(actual);
	}
}
