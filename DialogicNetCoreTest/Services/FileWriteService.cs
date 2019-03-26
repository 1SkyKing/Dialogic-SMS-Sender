using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DialogicNetCoreTest.Services
{
    public class FileWriteService : IHostedService,IDisposable
    {
        private const string Path = @"c:\temp\TestCoreService.txt";
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                (e) => WriteTimeToFile(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public void WriteTimeToFile()
        {
            var length = 140;
            for (int i = 0; i < length; i++)
            {
                if (!File.Exists(Path))
                {
                    using (var sw = File.CreateText(Path))
                    {
                        sw.WriteLine(DateTime.UtcNow.ToString("O"));
                    }
                }
                else
                {
                    using (var sw = File.AppendText(Path))
                    {
                        sw.WriteLine(DateTime.UtcNow.ToString("O"));
                    }
                }
            }
           
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
