namespace Tgs.GameInterface.SharpSports.Core.Models;

/// <param name="State">订单状态 0=确认中，1=下单成功，2=赢，3=输，4=和，5=取消，6=撤单，7=下单失败，8=作废单</param>
public sealed record class SportOrderListResponseItem(
	string LoginName,
	string OrderId,
	int State,
	string CashoutType,
	DateTimeOffset CreateDate,
	DateTimeOffset ResultDate,
	DateTimeOffset RealResultDate,
	decimal Gold,
	decimal RealGold,
	decimal WinGold,
	string AdditionalData);
