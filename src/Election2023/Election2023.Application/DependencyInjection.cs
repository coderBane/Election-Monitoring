using Microsoft.Extensions.DependencyInjection;

using Election2023.Application.Behaviors;

namespace Election2023.Application
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
		{
			// Mapster
			var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
			typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
			services.AddSingleton(typeAdapterConfig);
			services.AddScoped<IMapper, ServiceMapper>();

			services.AddMediatR(Assembly.GetExecutingAssembly());
			// services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
			// services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
			return services;
		}
	}
}

