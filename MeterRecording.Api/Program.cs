using MeterRecording.Application.Extensions;
using MeterRecording.Infrastructure.Database;
using MeterRecording.Infrastructure.Extensions;
using MeterRecording.Infrastructure.Seed.Accounts;
using Microsoft.EntityFrameworkCore;

namespace MeterRecording.Api
{
    public class Program
    {
        private const string ACCOUNT_SEED_PATH_CFG_KEY = "AccountSeedPath";
        private const string CLIENT_POLICY_NAME = "Client";

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

            // Allow react front-end to make request from different origin
            string origin = builder.Configuration.GetValue<string>(CLIENT_POLICY_NAME) ?? "http://localhost:3000";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(CLIENT_POLICY_NAME, builder =>
                    builder.WithOrigins(origin)
                            .AllowAnyHeader()
                            .AllowAnyMethod());
            });

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
            app.UseCors(CLIENT_POLICY_NAME);
            app.MapControllers();
            app.Run();
        }
    }
}
