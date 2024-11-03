using MeterRecording.Application.DTOs;

namespace MeterRecording.Application.Interfaces
{
    /// <summary>
    /// Processes meter readings
    /// </summary>
    public interface IMeterProcessingService
    {
        /// <summary>
        /// Process meter readings
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ProcessMeterReadingResultsDto> ProcessMeterReadingsAsync(Stream stream, CancellationToken cancellationToken);
    }
}
