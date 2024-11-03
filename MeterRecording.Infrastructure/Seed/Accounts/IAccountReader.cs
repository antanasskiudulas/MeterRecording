using MeterRecording.Core.Entities;

namespace MeterRecording.Infrastructure.Seed.Accounts
{
    /// <summary>
    /// Methods for reading account
    /// </summary>
    public interface IAccountReader
    {
        IEnumerable<Account> ReadAccounts(string location);
    }
}
