namespace MeterRecording.Application.DTOs
{
    /// <summary>
    /// Represents meter reading output
    /// </summary>
    public class ProcessMeterReadingResultsDto
    {
        /// <summary>
        /// Number of successfully read records
        /// </summary>
        public int SuccessReadings { get; set; }

        /// <summary>
        /// Number of unsuccessfully read records
        /// </summary>
        public int FailedReadings { get; set; }
    }
}
