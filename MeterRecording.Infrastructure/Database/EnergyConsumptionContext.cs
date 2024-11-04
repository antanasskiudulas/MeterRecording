using MeterRecording.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterRecording.Infrastructure.Database
{
    /// <summary>
    /// Database context for energy consumption
    /// </summary>
    public class EnergyConsumptionContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }

        public EnergyConsumptionContext(DbContextOptions<EnergyConsumptionContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MeterReading>()
                .HasOne<Account>()
                .WithMany()
                .HasForeignKey(m => m.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MeterReading>()
                .Property(x => x.ReadTime)
                .HasConversion(
                    w => DateTime.SpecifyKind(w, DateTimeKind.Utc),
                    r => DateTime.SpecifyKind(r, DateTimeKind.Utc));
        }
    }
}
