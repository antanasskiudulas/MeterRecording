using MeterRecording.Application.DTOs;
using MeterRecording.Application.Interfaces;
using MeterRecording.Core.Entities;
using MeterRecording.Core.Interfaces.Readers;
using MeterRecording.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace MeterRecording.Application.Services
{
    /// <inheritdoc cref="IMeterProcessingService"/>
    public class MeterProcessingService : IMeterProcessingService
    {
        private readonly IMeterReader _meterReader;
        private readonly IAccountRepository _accountRepository;
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly ILogger<MeterProcessingService> _logger;

        public MeterProcessingService(
            IMeterReader meterReader,
            IAccountRepository accountRepository,
            IMeterReadingRepository meterReadingRepository,
            ILogger<MeterProcessingService> logger)
        {
            _meterReader = meterReader;
            _accountRepository = accountRepository;
            _meterReadingRepository = meterReadingRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ProcessMeterReadingResultsDto> ProcessMeterReadingsAsync(Stream stream, CancellationToken cancellationToken)
        {
            (List<MeterReading> validatedRecords, int invalidRecords) = await GetValidatedReadings(stream, cancellationToken);

            await _meterReadingRepository.InsertReadings(validatedRecords, cancellationToken);

            return new ProcessMeterReadingResultsDto
            {
                SuccessReadings = validatedRecords.Count(),
                FailedReadings = invalidRecords
            };
        }

        private async Task<(List<MeterReading>, int)> GetValidatedReadings(Stream stream, CancellationToken cancellationToken)
        {
            (List<MeterReading> validFormatReadings, int malformedReadings) = await ReadValidFormatMeterRecords(stream, cancellationToken);

            (List<MeterReading> meterReadings, int unassociatedReadings) = await GetOnlyAccountAssociatedReadings(validFormatReadings, cancellationToken);

            (List<MeterReading> validMeterReadings, int outdatedReadings) = await GetChronologicallyValidReadings(meterReadings, cancellationToken);

            return (validMeterReadings, malformedReadings + unassociatedReadings + outdatedReadings);
        }

        private async Task<(List<MeterReading>, int)> ReadValidFormatMeterRecords(Stream stream, CancellationToken cancellationToken)
        {
            List<MeterReading> validFormatMeterReadings = new List<MeterReading>();

            int malformedReadings = 0;
            await foreach (MeterReading reading in _meterReader.ReadMeterAsync(stream, cancellationToken))
            {
                if (reading.ReadValue < 0)
                {
                    LogSkip(reading, "Expected reading to be >= 0");
                    malformedReadings++;

                    continue;
                }

                if (reading.ReadValue.ToString(CultureInfo.InvariantCulture).Length > 5)
                {
                    LogSkip(reading, $"Expected file reading to be of format NNNNN, but was [{reading.ReadValue}]");
                    malformedReadings++;

                    continue;
                }

                validFormatMeterReadings.Add(reading);
            }

            return (validFormatMeterReadings, malformedReadings);
        }

        private async Task<(List<MeterReading>, int)> GetOnlyAccountAssociatedReadings(List<MeterReading> validFormatReadings, CancellationToken cancellationToken)
        {
            //TODO: consider maybe caching decorator for repository
            HashSet<int> accountIds = new HashSet<int>(await _accountRepository.GetAllAccountIds(cancellationToken));
            List<MeterReading> readings = new List<MeterReading>();

            int failedReadings = 0;
            foreach (MeterReading reading in validFormatReadings)
            {
                if (!accountIds.TryGetValue(reading.AccountId, out int _))
                {
                    LogSkip(reading, "Could not find associated account");
                    failedReadings++;

                    continue;
                }

                readings.Add(reading);
            }

            return (readings, failedReadings);
        }

        private async Task<(List<MeterReading>, int)> GetChronologicallyValidReadings(List<MeterReading> meterReadings, CancellationToken cancellationToken)
        {
            List<MeterReading> validMeterReadings = new List<MeterReading>();
            Dictionary<int, DateTime> latestReadingTimesPerAccount = await _meterReadingRepository.GetLatestReadTimesForAccounts(
                meterReadings.Select(x => x.AccountId),
                cancellationToken);

            int outdatedReadings = 0;

            foreach (MeterReading meterReading in meterReadings)
            {
                if (!latestReadingTimesPerAccount.TryGetValue(meterReading.AccountId, out DateTime latestMeterReadingTime))
                {
                    latestReadingTimesPerAccount.Add(meterReading.AccountId, meterReading.ReadTime);
                }
                else if (meterReading.ReadTime <= latestMeterReadingTime)
                {
                    LogSkip(meterReading, "Entry is outdated");
                    outdatedReadings++;

                    continue;
                }

                validMeterReadings.Add(meterReading);
                latestReadingTimesPerAccount[meterReading.AccountId] = meterReading.ReadTime;
            }

            return (validMeterReadings, outdatedReadings);
        }

        private void LogSkip(MeterReading reading, string reason)
        {
            _logger.LogWarning($"Skipped processing meter reading with Account Id [{reading.AccountId}] time stamped {reading.ReadTime}."
                + $" Reason: {reason}");
        }
    }
}
