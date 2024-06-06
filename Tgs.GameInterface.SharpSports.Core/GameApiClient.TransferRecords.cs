using System.Text.Json.Nodes;

namespace Tgs.GameInterface.SharpSports.Core;

internal sealed partial class GameApiClient
{
	public async ValueTask<bool> TransferRecordsAsync(
		Uri apiBaseUrl,
		string merchantId,
		string clientId,
		string clientSecret,
		string loginName,
		DateTimeOffset startTime,
		DateTimeOffset endTime,
		int pageNum,
		int pageSize,
		string merchantPayNo,
		CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["loginName"] = loginName,
			["startTime"] = _dateTimeConverter.DatetimeConvertToString(startTime),
			["endTime"] = _dateTimeConverter.DatetimeConvertToString(endTime),
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
			["merchantPayNo"] = new JsonArray
			{
				merchantPayNo
			},
		};
		var jsonNode = await PostAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			relativeUrl: "/api/transfer/records",
			parameters: parameters,
			logPrefix: nameof(TransferRecordsAsync),
			cancellationToken)
			.ConfigureAwait(false);

		var code = jsonNode["code"]!.GetValue<int>();
		if (code != 200)
			throw new ErrorCodeException(code, jsonNode["msg"]?.GetValue<string>() ?? Code2Message(code));

		foreach (var nullableNode in jsonNode["data"]!["list"]!.AsArray())
		{
			var type = nullableNode!["tradeType"]!.GetValue<string>();
			if (type.Equals("RECHARGE", StringComparison.InvariantCultureIgnoreCase) ||
				type.Equals("DRAW", StringComparison.InvariantCultureIgnoreCase))
				return true;
		}

		return false;

		static string Code2Message(int code) => code switch
		{
			400100 => "请填写必填字段",
			400101 => "参数类型错误",
			400103 => "会员账号不符合规范",
			400104 => "查询时间超出限制",
			400105 => "tradeType不符合规范",
			400106 => "merchantPayNo不符合规范",
			400108 => "日期参数错误",
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
