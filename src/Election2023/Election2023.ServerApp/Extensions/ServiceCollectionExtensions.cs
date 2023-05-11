using Election2023.ServerApp.Services;
using Election2023.Application.Interfaces.Services;

using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Election2023.ServerApp.Extensions;

internal static class ServiceCollectionExtensions
{
	internal static IServiceCollection AddCurrentUserService(this IServiceCollection services)
	{
        services.AddScoped<ICurrentUserService, CurrentUserService>();
		services.TryAddEnumerable(ServiceDescriptor.Scoped<CircuitHandler, UserCircuitHandler>());
        return services;
	}
}

