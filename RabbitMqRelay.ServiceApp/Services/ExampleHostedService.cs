using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMqRelay.ServiceApp.Models;

namespace RabbitMqRelay.ServiceApp.Services
{
    public class ExampleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly MessageRouting _messageRouting;

        public ExampleHostedService(
            ILogger<ExampleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            IOptions<MessageRouting> opts)
        {
            _logger = logger;

            _messageRouting = opts.Value;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("1. StartAsync has been called.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("4. StopAsync has been called.");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("2. OnStarted has been called.");
        }

        private void OnStopping()
        {
            _logger.LogInformation("3. OnStopping has been called.");
        }

        private void OnStopped()
        {
            _logger.LogInformation("5. OnStopped has been called.");
        }
    }
}