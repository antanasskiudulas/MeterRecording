using CsvHelper;
using CsvHelper.Configuration;
using MeterRecording.Core.Entities;
using System.Globalization;

namespace MeterRecording.Infrastructure.Seed.Accounts.CSV
{
    /// <inheritdoc cref="IAccountReader"/>
    public class CsvAccountReader : IAccountReader
    {
        /// <inheritdoc/>
        public IEnumerable<Account> ReadAccounts(string location)
        {
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            using (StreamReader reader = new StreamReader(location))
            using (CsvReader csvReader = new CsvReader(reader, config))
            {
                csvReader.Context.RegisterClassMap<AccountMap>();
                return csvReader.GetRecords<Account>().ToList();
            }
        }
    }
}
