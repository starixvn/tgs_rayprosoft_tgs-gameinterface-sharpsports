namespace Tgs.GameInterface.SharpSports.Core;

public enum SportOrderListState
{
	/// <summary>
	/// The waiting
	/// 0=確認中
	/// </summary>
	Waiting = 0,

	/// <summary>
	/// The running
	/// 1=下單成功
	/// </summary>
	Running = 1,

	/// <summary>
	/// The win
	/// 2=贏
	/// </summary>
	Win = 2,

	/// <summary>
	/// The loss
	/// 3=輸
	/// </summary>
	Loss = 3,

	/// <summary>
	/// The draw
	/// 4=和
	/// </summary>
	Draw = 4,

	/// <summary>
	/// The cancel
	/// 5=取消
	/// </summary>
	Cancel = 5,

	/// <summary>
	/// The cancel order
	/// 6=撤單
	/// </summary>
	CancelOrder = 6,

	/// <summary>
	/// The failed order
	/// 7=下單失敗
	/// </summary>
	FailedOrder = 7,

	/// <summary>
	/// The void
	/// 8=作廢單
	/// </summary>
	Void = 8
}
