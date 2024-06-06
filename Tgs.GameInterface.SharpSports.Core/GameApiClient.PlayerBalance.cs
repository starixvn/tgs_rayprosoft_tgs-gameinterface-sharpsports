using System.Text.Json.Nodes;
using Tgs.GameInterface.SharpSports.Core.Models;

namespace Tgs.GameInterface.SharpSports.Core;

internal sealed partial class GameApiClient
{
	public async ValueTask<PlayerBalanceResponse> PlayerBalanceAsync(
		Uri apiBaseUrl,
		string merchantId,
		string clientId,
		string clientSecret,
		string loginName,
		CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
		};
		var jsonNode = await PostAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			relativeUrl: "/api/player/balance",
			parameters: parameters,
			logPrefix: nameof(PlayerBalanceAsync),
			cancellationToken)
			.ConfigureAwait(false);

		var code = jsonNode["code"]!.GetValue<int>();
		if (code != 200)
		{
			return code != 400104
				? throw new ErrorCodeException(code, jsonNode["msg"]?.GetValue<string>() ?? Code2Message(code))
				: new PlayerBalanceResponse(
					IsPlayerExists: false,
					Balance: 0.0m);
		}

		var data = jsonNode["data"]!;

		return new PlayerBalanceResponse(
			IsPlayerExists: true,
			Balance: data["balance"]!.GetValue<decimal>());

		static string Code2Message(int code) => code switch
		{
			400100 => "请填写必填字段",
			400101 => "参数类型错误",
			400103 => "会员账号不符合规范",
			400110 => "当前账号已禁用",
			400111 => "当前账号已冻结",
			400200 => "客户端应用不存在",
			400201 => "客户端秘钥错误",
			400202 => "客户端商户与接入商户不匹配",
			400203 => "客户端商户不存在",
			400204 => "客户端密文解密失败",
			400205 => "当前商户已禁用",
			_ => $"{code}",
		};
	}
}
