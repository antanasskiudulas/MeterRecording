namespace MeterRecording.Infrastructure.Seed.Accounts
{
    /// <summary>
    /// Methods for seeding database with entries
    /// </summary>
    public interface ISeeder
    {
        /// <summary>
        /// Seed to database
        /// </summary>
        /// <param name="location">Accounts location</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task SeedAsync(string location, CancellationToken cancellationToken);
    }
}
