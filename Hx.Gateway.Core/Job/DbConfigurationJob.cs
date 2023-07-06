using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.Job
{
    public class DbConfigurationJob : IJob
    {
        private readonly IOcelotLogger _logger;
        private readonly IServiceProvider _serviceProvider;
        public DbConfigurationJob(IOcelotLogger logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            
            using var scope = _serviceProvider.CreateScope();
            IFileConfigurationRepository _rep = scope.ServiceProvider.GetRequiredService<IFileConfigurationRepository>();
            IInternalConfigurationRepository _internalConfigRep = scope.ServiceProvider.GetRequiredService<IInternalConfigurationRepository>();
            IInternalConfigurationCreator _internalConfigCreator = scope.ServiceProvider.GetRequiredService<IInternalConfigurationCreator>();
            var fileConfig = await _rep.Get();
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
                    _internalConfigRep.AddOrReplace(config.Data);
                }
            }
        }
    }
}
