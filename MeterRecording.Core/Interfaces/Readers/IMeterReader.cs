using MeterRecording.Core.Entities;

namespace MeterRecording.Core.Interfaces.Readers
{
    /// <summary>
    /// Reads meter records
    /// </summary>
    public interface IMeterReader
    {
        /// <summary>
        /// Reads <see cref="MeterReading"/> from a given stream
        /// </summary>
        /// <param name="stream">Stream the readings to be read from</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        public IAsyncEnumerable<MeterReading> ReadMeterAsync(Stream stream, CancellationToken cancellationToken);
    }
}
