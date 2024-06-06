using System.Reflection;
using Tgs.GameInterface.SharpSports.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameInterfaceCore(
		this IServiceCollection services)
		=> services
		.AddSingleton<DateTimeConverter>()
		.AddSingleton<IParameterEncryptor, ParameterEncryptor>()
		.AddHttpClient<IGameApiClient, GameApiClient>().Services
		.RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
		.AddSingleton(TimeProvider.System);
}
