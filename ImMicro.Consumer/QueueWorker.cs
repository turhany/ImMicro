using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace ImMicro.Consumer
{
    public class QueueWorker : IHostedService
    {
        private readonly IBusControl _initBus;

        public QueueWorker(IBusControl busControl)
        {
            _initBus = busControl;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _initBus.StartAsync(cancellationToken);
            Console.WriteLine(@"Listening for queued messages");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _initBus.StopAsync(cancellationToken);
            Console.WriteLine(@"Stopping read queue message");
        }
    }
}