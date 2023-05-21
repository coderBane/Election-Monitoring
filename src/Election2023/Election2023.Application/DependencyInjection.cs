using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Election2023.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
		{
			services.AddMediatR(Assembly.GetExecutingAssembly());
			return services;
		}
	}
}

