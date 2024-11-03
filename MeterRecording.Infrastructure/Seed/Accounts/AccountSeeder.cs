using MeterRecording.Core.Entities;
using MeterRecording.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace MeterRecording.Infrastructure.Seed.Accounts
{
    /// <inheritdoc cref="ISeeder"/>
    public class AccountSeeder : ISeeder
    {
        private readonly ILogger<AccountSeeder> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountReader _accountReader;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountSeeder(
            IAccountRepository accountRepository,
            IAccountReader accountReader,
            ILogger<AccountSeeder> logger)
        {
            _accountRepository = accountRepository;
            _accountReader = accountReader;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task SeedAsync(string location, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(location))
                {
                    throw new ArgumentException(nameof(location));
                }

                if (await _accountRepository.IsSeeded(cancellationToken))
                {
                    _logger.LogInformation($"Accounts are already seeded");
                    return;
                }

                IEnumerable<Account> accounts = _accountReader.ReadAccounts(location);
                await _accountRepository.InsertAccounts(accounts.OrderBy(x => x.Id), cancellationToken);

                _logger.LogInformation($"Seeded {accounts.Count()} accounts");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while seeding Accounts table");
            }
        }
    }
}
