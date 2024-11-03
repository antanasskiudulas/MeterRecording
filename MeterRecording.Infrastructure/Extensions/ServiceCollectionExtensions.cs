using MeterRecording.Core.Interfaces.Repositories;
using MeterRecording.Infrastructure.Data;
using MeterRecording.Infrastructure.Repositories;
using MeterRecording.Infrastructure.Seed.Accounts;
using MeterRecording.Infrastructure.Seed.Accounts.CSV;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeterRecording.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private const string DEV_CONNECTION_STRING_KEY = "EnergyConsumptionDatabase";

        /// <summary>
        /// Add energy consumption database configuration to service collection
        /// </summary>
        /// <param name="services">Services</param>
        /// <param name="configuration">Configuration</param>
        public static IServiceCollection AddEnergyConsumptionDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<EnergyConsumptionContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString(DEV_CONNECTION_STRING_KEY),
                    b => b.MigrationsAssembly(typeof(EnergyConsumptionContext).Assembly.FullName)));

            return services;
        }

        /// <summary>
        /// Adds bindings for infrastructure related services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IAccountReader, CsvAccountReader>();
            services.AddSingleton<ISeeder, AccountSeeder>();

            return services;
        }
    }
}
