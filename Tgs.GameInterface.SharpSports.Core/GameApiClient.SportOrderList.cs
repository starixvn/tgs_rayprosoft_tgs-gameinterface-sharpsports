using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Tgs.GameInterface.SharpSports.Core.Models;

namespace Tgs.GameInterface.SharpSports.Core;

internal sealed partial class GameApiClient
{
	public async IAsyncEnumerable<SportOrderListResponseItem> SportOrderListAsync(
		Uri apiBaseUrl,
		string merchantId,
		string clientId,
		string clientSecret,
		DateTimeOffset beginTime,
		DateTimeOffset endTime,
		SportOrderListTimeType timeType,
		int pageNum,
		int pageSize,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var parameters = new JsonObject
		{
			["beginTime"] = _dateTimeConverter.DatetimeConvertToString(beginTime),
			["endTime"] = _dateTimeConverter.DatetimeConvertToString(endTime),
			["timeType"] = (byte)timeType,
			["pageNum"] = pageNum,
			["pageSize"] = pageSize,
		};
		var jsonNode = await PostAsync(
			apiBaseUrl,
			merchantId,
			clientId,
			clientSecret,
			relativeUrl: "/api/sport/orderList",
			parameters: parameters,
			logPrefix: nameof(SportOrderListAsync),
			cancellationToken)
			.ConfigureAwait(false);

		var code = jsonNode["code"]!.GetValue<int>();
		if (code != 200)
			throw new ErrorCodeException(code, jsonNode["msg"]?.GetValue<string>() ?? Code2Message(code));

		var data = jsonNode["data"]!;
		foreach (var nullableNode in data["orderDTO"]!.AsArray())
		{
			var node = nullableNode!;
			var createDateStr = node["createDate"]?.GetValue<string>();
			var resultDateStr = node["resultDate"]?.GetValue<string>();
			var realResultDateStr = node["realResultDate"]?.GetValue<string>();
			yield return new(
				LoginName: node["loginName"]!.GetValue<string>(),
				OrderId: node["orderId"]!.GetValue<string>(),
				State: node["state"]!.GetValue<int>(),
				CashoutType: node["cashoutType"]!.GetValue<string>(),
				CreateDate: createDateStr is null ? DateTimeOffset.UnixEpoch : _dateTimeConverter.StringConvertToDatetime(createDateStr),
				ResultDate: resultDateStr is null ? DateTimeOffset.UnixEpoch : _dateTimeConverter.StringConvertToDatetime(resultDateStr),
				RealResultDate: realResultDateStr is null ? DateTimeOffset.UnixEpoch : _dateTimeConverter.StringConvertToDatetime(realResultDateStr),
				Gold: node["gold"]!.GetValue<decimal>(),
				RealGold: node["validAmount"]?.GetValue<decimal>() ?? node["realGold"]!.GetValue<decimal>(),
				WinGold: node["winGold"]?.GetValue<decimal>() ?? 0m,
				AdditionalData: node.ToJsonString());
		}

		static string Code2Message(int code) => code switch
		{
			400100 => "请填写必填字段",
			400101 => "参数类型错误",
			400103 => "会员账号不符合规范",
			400109 => "金额必须大于1和不超过10000之间",
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
