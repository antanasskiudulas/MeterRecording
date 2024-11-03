using MeterRecording.Core.Entities;
using MeterRecording.Core.Interfaces.Repositories;
using MeterRecording.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeterRecording.Infrastructure.Repositories
{
    /// <inheritdoc cref="IAccountRepository"/>
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbContextFactory<EnergyConsumptionContext> _contextFactory;

        public AccountRepository(IDbContextFactory<EnergyConsumptionContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <inheritdoc/>
        public async Task InsertAccounts(IEnumerable<Account> accounts, CancellationToken cancellationToken)
        {
            using (EnergyConsumptionContext context = await _contextFactory.CreateDbContextAsync(cancellationToken))
            {
                await context.Accounts.AddRangeAsync(accounts);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> IsSeeded(CancellationToken cancellationToken)
        {
            using (EnergyConsumptionContext context = await _contextFactory.CreateDbContextAsync(cancellationToken))
            {
                return await context.Accounts.AnyAsync(cancellationToken);
            }
        }
    }
}
