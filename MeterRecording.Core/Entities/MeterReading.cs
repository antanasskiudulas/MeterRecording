namespace MeterRecording.Core.Entities
{
    /// <summary>
    /// Represents a meter reading
    /// </summary>
    public class MeterReading
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID relating a reading to <see cref="Account"/>
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Time stamp of the reading
        /// </summary>
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// Energy meter read value
        /// </summary>
        public int ReadValue { get; set; }
    }
}
