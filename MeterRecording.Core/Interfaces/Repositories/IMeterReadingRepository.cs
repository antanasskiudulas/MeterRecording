using MeterRecording.Core.Entities;

namespace MeterRecording.Core.Interfaces.Repositories
{
    /// <summary>
    /// Defines access to meter readings
    /// </summary>
    public interface IMeterReadingRepository
    {
        /// <summary>
        /// Get latest read times for accounts
        /// </summary>
        /// <param name="ids">IDs of accounts</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Latest reading time stamps keyed by account ID</returns>
        Task<Dictionary<int, DateTime>> GetLatestReadTimesForAccounts(IEnumerable<int> ids, CancellationToken cancellationToken);

        /// <summary>
        /// Inserts a collection of meter readings
        /// </summary>
        /// <param name="meterReadings">Meter readings to be inserted</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task InsertReadings(IEnumerable<MeterReading> meterReadings, CancellationToken cancellationToken);
    }
}
