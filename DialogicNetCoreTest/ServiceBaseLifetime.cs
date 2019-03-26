using Microsoft.Extensions.Hosting;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace DialogicNetCoreTest
{
    // Code from https://github.com/aspnet/Hosting/blob/2a98db6a73512b8e36f55a1e6678461c34f4cc4d/samples/GenericHostSample/ServiceBaseLifetime.cs
    public class ServiceBaseLifetime:ServiceBase,IHostLifetime
    {
        private readonly TaskCompletionSource<object> _delayStart = new TaskCompletionSource<object>();

        public ServiceBaseLifetime(IApplicationLifetime applicationLifetime)
        {
            ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        }

        private IApplicationLifetime ApplicationLifetime { get; }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _delayStart.TrySetCanceled());
            ApplicationLifetime.ApplicationStopping.Register(Stop);

            new Thread(Run).Start(); //Aksi halde bu, IHost.StartAsync'in sonlandırılmasını engeller ve önler.
            return _delayStart.Task;
        }

        private void Run()
        {
            try
            {
                Run(this);// Bu hizmet durdurulana kadar engeller.
                _delayStart.TrySetException(new InvalidOperationException("Başlamadan Durdu"));
            }
            catch(Exception ex)
            {
                _delayStart.TrySetException(ex);
            }
        }

        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Stop();
            return Task.CompletedTask;
        }

        //Base tarafından çağrılır. Servis başlatılmaya hazır olduğunda çalışır.
        protected override void OnStart(string[] args)
        {
            _delayStart.TrySetResult(null);
            base.OnStart(args);
        }

        //Base Class dan çağrılır,Bu, servis durdurma, ApplicationStopping ve StopAsync servisi tarafından birçok kez çağrılabilir.
        //Sorun değil, çünkü StopApplication bir CancellationTokenSource kullanıyor ve yinelemeyi engelliyor.
        protected override void OnStop()
        {
            ApplicationLifetime.StopApplication();
            base.OnStop();
        }

    }
}
