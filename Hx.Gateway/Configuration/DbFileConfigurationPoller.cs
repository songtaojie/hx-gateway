using Hx.Gateway.Core.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.Configuration
{
    public class DbConfigurationPoller :BackgroundService
    {
        private readonly IOcelotLogger _logger;
        private readonly IFileConfigurationRepository _repo;
        private readonly OcelotSettingsOptions _ocelotSettings;
        private bool _polling;
        private readonly IInternalConfigurationRepository _internalConfigRepo;
        private readonly IInternalConfigurationCreator _internalConfigCreator;
        public DbConfigurationPoller(IOcelotLoggerFactory factory,
            IFileConfigurationRepository repo,
            IInternalConfigurationRepository internalConfigRepo,
            IInternalConfigurationCreator internalConfigCreator,
            IOptionsMonitor<OcelotSettingsOptions> option)
        {
            _internalConfigRepo = internalConfigRepo;
            _internalConfigCreator = internalConfigCreator;
            _logger = factory.CreateLogger<DbConfigurationPoller>();
            _repo = repo;
            _ocelotSettings = option.CurrentValue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(DbConfigurationPoller)} is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_ocelotSettings.EnableTimer && !_polling)
                {
                    _polling = true;
                    await Poll();
                    _polling = false;
                }
                await Task.Delay(_ocelotSettings.TimerDelay, stoppingToken);
            }
        }

        private async Task Poll()
        {
            _logger.LogInformation("Started polling");

            var fileConfig = await _repo.Get();

            if (fileConfig.IsError)
            {
                _logger.LogWarning($"error geting file config, errors are {string.Join(",", fileConfig.Errors.Select(x => x.Message))}");
                return;
            }
            else
            {
                var config = await _internalConfigCreator.Create(fileConfig.Data);
                if (!config.IsError)
                {
                    _internalConfigRepo.AddOrReplace(config.Data);
                }
            }
            _logger.LogInformation("Finished polling");
        }
    }
}
