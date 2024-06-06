using System.Text.Json.Nodes;

namespace Tgs.GameInterface.SharpSports.Core;

internal sealed partial class GameApiClient
{
	public async ValueTask<string> PlayerSportsLoginAsync(
		Uri apiBaseUrl,
		string merchantId,
		string clientId,
		string clientSecret,
		string loginName,
		TerType terType,
		string language,
		string ip,
		CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["terType"] = (byte)terType,
			["language"] = language,
			["ip"] = ip,
		};
		var jsonNode = await PostAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			relativeUrl: "/api/player/sportsLogin",
			parameters: parameters,
			logPrefix: nameof(PlayerSportsLoginAsync),
			cancellationToken)
			.ConfigureAwait(false);

		var code = jsonNode["code"]!.GetValue<int>();
		if (code != 200)
			throw new ErrorCodeException(code, jsonNode["msg"]?.GetValue<string>() ?? Code2Message(code));

		var data = jsonNode["data"]!;
		return data["gameLink"]!.GetValue<string>();

		static string Code2Message(int code) => code switch
		{
			400100 => "请填写必填字段",
			400101 => "参数类型错误",
			400103 => "会员账号不符合规范",
			400112 => "会员昵称不符合规范",
			400200 => "客户端应用不存在",
			400201 => "客户端秘钥错误",
			400202 => "客户端商户与接入商户不匹配",
			400203 => "客户端商户不存在",
			400204 => "客户端密文解密失败",
			400205 => "当前商户已禁用",
			400208 => "当前商户配置不存在",
			400209 => "当前商户配置已禁用",
			_ => $"{code}",
		};
	}
}
