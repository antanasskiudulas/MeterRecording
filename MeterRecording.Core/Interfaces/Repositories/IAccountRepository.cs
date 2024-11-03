using MeterRecording.Core.Entities;

namespace MeterRecording.Core.Interfaces.Repositories
{
    /// <summary>
    /// Methods for accessing accounts
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Insert a collection of accounts
        /// </summary>
        /// <param name="accounts">Collection of accounts to be inserted</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task InsertAccounts(IEnumerable<Account> accounts, CancellationToken cancellationToken);

        /// <summary>
        /// Check if accounts table is already seeded
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<bool> IsSeeded(CancellationToken cancellationToken);
    }
}
