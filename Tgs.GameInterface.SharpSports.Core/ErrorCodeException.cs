namespace Tgs.GameInterface.SharpSports.Core;

public sealed class ErrorCodeException(int code, string message) : Exception($"{code}: {message}")
{
	public int Code => code;
}
