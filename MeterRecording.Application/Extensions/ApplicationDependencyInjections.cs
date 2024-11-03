using MeterRecording.Application.Interfaces;
using MeterRecording.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeterRecording.Application.Extensions
{
    /// <summary>
    /// Extension methods for <see cref=""/> to facilitate modular dependency injection for application layer
    /// </summary>
    public static class ApplicationDependencyInjections
    {
        /// <summary>
        /// Adds application layer services
        /// </summary>
        /// <param name="services">Service collection to add it to</param>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IMeterProcessingService, MeterProcessingService>();

            return services;
        }
    }
}
