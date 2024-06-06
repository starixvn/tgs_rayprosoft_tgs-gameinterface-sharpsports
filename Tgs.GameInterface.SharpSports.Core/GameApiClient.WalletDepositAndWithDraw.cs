using System.Text.Json.Nodes;
using Tgs.GameInterface.SharpSports.Core.Models;

namespace Tgs.GameInterface.SharpSports.Core;

internal sealed partial class GameApiClient
{
	public async ValueTask<WalletDepositAndWithDrawResponse> WalletDepositAndWithDrawAsync(
		Uri apiBaseUrl,
		string merchantId,
		string clientId,
		string clientSecret,
		string loginName,
		string merchantPayNo,
		TradeType tradeType,
		decimal amount,
		CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["merchantPayNo"] = merchantPayNo,
			["tradeType"] = (byte)tradeType,
			["amount"] = amount,
		};
		var jsonNode = await PostAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			relativeUrl: "/api/wallet/depositAndWithDraw",
			parameters: parameters,
			logPrefix: nameof(WalletDepositAndWithDrawAsync),
			cancellationToken)
			.ConfigureAwait(false);

		var code = jsonNode["code"]!.GetValue<int>();
		if (code != 200)
			throw new ErrorCodeException(code, jsonNode["msg"]?.GetValue<string>() ?? Code2Message(code));

		var data = jsonNode["data"]!;
		return new(
			TradeNo: data["tradeNo"]!.GetValue<string>(),
			MerchantPayNo: data["merchantPayNo"]!.GetValue<string>(),
			Balance: data["balance"]!.GetValue<decimal>(),
			Resp: data.ToJsonString());

		static string Code2Message(int code) => code switch
		{
			400100 => "请填写必填字段",
			400101 => "参数类型错误",
			400103 => "会员账号不符合规范",
			400104 => "未查找到会员账号信息",
			400105 => "tradeType不符合规范",
			400106 => "创建交易失败",
			400107 => "merchantPayNo不符合规范",
			400108 => "会员账号资金被冻结",
			400109 => "金额必须在大于1和不超过99999999之间",
			400110 => "当前账号已禁用",
			400111 => "当前账号已冻结",
			400112 => "会员昵称不符合规范",
			400200 => "客户端应用不存在",
			400201 => "客户端秘钥错误",
			400202 => "客户端商户与接入商户不匹配",
			400203 => "客户端商户不存在",
			400204 => "客户端密文解密失败",
			400205 => "当前商户已禁用",
			500107 => "CNY钱包异常:用户账户余额不足!",
			500129 => "玩家试玩状态启用！",
			_ => $"{code}",
		};
	}
}
