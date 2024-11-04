using MeterRecording.Core.Entities;
using MeterRecording.Core.Interfaces.Repositories;
using MeterRecording.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MeterRecording.Infrastructure.Repositories
{
    /// <inheritdoc cref="IMeterReadingRepository"/>
    public class MeterReadingRepository : IMeterReadingRepository
    {
        IDbContextFactory<EnergyConsumptionContext> _dbContextFactory;

        public MeterReadingRepository(IDbContextFactory<EnergyConsumptionContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<int, DateTime>> GetLatestReadTimesForAccounts(IEnumerable<int> accountIds, CancellationToken cancellationToken)
        {
            using (EnergyConsumptionContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                return await context.MeterReadings
                    .Where(x => accountIds.Contains(x.AccountId))
                    .GroupBy(x => x.AccountId)
                    .Select(g => new
                    {
                        AccountId = g.Key,
                        LatestReadTime = g.Max(x => x.ReadTime)
                    })
                    .ToDictionaryAsync(
                        k => k.AccountId,
                        v => v.LatestReadTime,
                        cancellationToken);
            }
        }

        /// <inheritdoc/>
        public async Task InsertReadings(IEnumerable<MeterReading> meterReadings, CancellationToken cancellationToken)
        {
            using (EnergyConsumptionContext context = await _dbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                await context.MeterReadings.AddRangeAsync(meterReadings, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
