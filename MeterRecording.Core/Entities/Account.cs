namespace MeterRecording.Core.Entities
{
    /// <summary>
    /// Represents an account
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Account ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;
    }
}
