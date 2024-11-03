using CsvHelper;
using CsvHelper.Configuration;
using MeterRecording.Core.Entities;
using MeterRecording.Core.Interfaces.Readers;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MeterRecording.Infrastructure.Readers
{
    ///<inheritdoc cref="IMeterReader"/>
    public class CsvMeterReader : IMeterReader
    {
        ILogger<CsvMeterReader> _logger;

        public CsvMeterReader(ILogger<CsvMeterReader> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<MeterReading> ReadMeterAsync(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                ReadingExceptionOccurred = args =>
                {
                    _logger.LogError(args.Exception, $"Error processing record at Row: {args.Exception.Context?.Parser?.Row}");

                    return false;
                }
            };

            using StreamReader reader = new StreamReader(stream);
            using (CsvReader csvReader = new CsvReader(reader, config))
            {
                csvReader.Context.RegisterClassMap<MeterReadingMap>();

                await foreach (MeterReading reading in csvReader.GetRecordsAsync<MeterReading>(cancellationToken))
                {
                    yield return reading;
                }
            };
        }
    }

    public sealed class MeterReadingMap : ClassMap<MeterReading>
    {
        public MeterReadingMap()
        {
            Map(x => x.AccountId).Name("AccountId");
            Map(x => x.ReadTime).Name("MeterReadingDateTime").TypeConverterOption.Format("dd/MM/yyyy HH:mm");
            Map(x => x.ReadValue).Name("MeterReadValue");
        }
    }
}
