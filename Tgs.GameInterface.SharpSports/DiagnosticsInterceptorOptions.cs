using System.Diagnostics;

namespace Tgs.GameInterface.SharpSports;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.SharpSports.DiagnosticsInterceptor");
}
