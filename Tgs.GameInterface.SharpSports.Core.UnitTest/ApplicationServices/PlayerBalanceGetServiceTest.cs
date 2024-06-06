using NSubstitute;
using Tgs.GameInterface.SharpSports.Core.ApplicationServices;

namespace Tgs.GameInterface.SharpSports.Core.UnitTest.ApplicationServices;

public sealed class PlayerBalanceGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new("https://api.yuansports1.com/"),
			MerchantId: "c6a33e4e235e731b51d2f303199edeb4",
			ClientId: "2a954fedf9a203d646e1a7f3c32528df",
			ClientSecret: "1ff45c6ed7494b04940dc0bd9e0dca6b",
			PlayerCode: "4ucmqplxb029");
		var response = new Models.PlayerBalanceResponse(
			true,
			   60m);
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.PlayerBalanceAsync(
				apiBaseUrl: Arg.Is(new Uri("https://api.yuansports1.com/")),
				merchantId: Arg.Is("c6a33e4e235e731b51d2f303199edeb4"),
				clientId: Arg.Is("2a954fedf9a203d646e1a7f3c32528df"),
				clientSecret: Arg.Is("1ff45c6ed7494b04940dc0bd9e0dca6b"),
				loginName: Arg.Is(query.PlayerCode))
			.Returns(ValueTask.FromResult(response));
		var sut = new PlayerBalanceGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query).ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExists);
		Assert.Equal(60m, actual.Balance);
	}
}
