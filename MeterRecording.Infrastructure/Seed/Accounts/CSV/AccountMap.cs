using CsvHelper.Configuration;
using MeterRecording.Core.Entities;

namespace MeterRecording.Infrastructure.Seed.Accounts.CSV
{
    /// <summary>
    /// Map class for <see cref="CsvAccountReader"/>
    /// </summary>
    public sealed class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Map(m => m.Id).Name("AccountId");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.LastName).Name("LastName");
        }
    }
}
