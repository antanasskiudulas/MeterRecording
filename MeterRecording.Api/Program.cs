using MeterRecording.Application.Extensions;
using MeterRecording.Infrastructure.Data;
using MeterRecording.Infrastructure.Extensions;
using MeterRecording.Infrastructure.Seed.Accounts;
using Microsoft.EntityFrameworkCore;

namespace MeterRecording.Api
{
    public class Program
    {
        private const string ACCOUNT_SEED_PATH_CFG_KEY = "AccountSeedPath";

        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Set appropriate config
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            // Add services to the container.
            builder.Services.AddControllers();

            // Service bindings
            builder.Services.AddEnergyConsumptionDbConfiguration(builder.Configuration);
            builder.Services.AddInfrastructureServices();
            builder.Services.AddApplicationServices();

            WebApplication app = builder.Build();

            // Apply pending migrations
            using (IServiceScope scope = app.Services.CreateScope())
            {
                IDbContextFactory<EnergyConsumptionContext> dbContextFactory = scope
                    .ServiceProvider
                    .GetRequiredService<IDbContextFactory<EnergyConsumptionContext>>();

                using (EnergyConsumptionContext dbContext = await dbContextFactory.CreateDbContextAsync())
                {
                    dbContext.Database.Migrate();
                }

                // Seed accounts table
                ISeeder seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();

                string? accountsPath = app.Configuration
                    .GetValue(ACCOUNT_SEED_PATH_CFG_KEY, string.Empty);

                await seeder.SeedAsync(accountsPath ?? string.Empty, CancellationToken.None);
            }

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
