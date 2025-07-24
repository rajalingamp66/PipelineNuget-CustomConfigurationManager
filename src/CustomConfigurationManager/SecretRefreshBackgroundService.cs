using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomConfigurationManager
{
    public class SecretRefreshBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        private readonly TaskCompletionSource<bool> _firstRefreshDone = new();

        public Task FirstRefreshCompleted => _firstRefreshDone.Task;

        public SecretRefreshBackgroundService(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int intervalMinutes = 15;
            string? intervalStr = _config["SecretRefreshInterval:SecretRefreshIntervalMinutes"];
            if (!string.IsNullOrEmpty(intervalStr) && int.TryParse(intervalStr, out int parsed))
                intervalMinutes = parsed;

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"[{DateTime.Now}] Refreshing secrets...");

                using var scope = _serviceProvider.CreateScope();
                var secretName = _config["AWSSecretsManagerStoreKey:SecretName"];
                var accessKey = _config["AWSS3SecretStoreKey:YOUR_ACCESS_KEY_ID"];
                var secretKey = _config["AWSS3SecretStoreKey:YOUR_SECRET_ACCESS_KEY"];

                var secretsService = scope.ServiceProvider.GetRequiredService<AWSSecretsManagerKeyReadService>();
                var result = await secretsService.RetrieveSecretsManagerSecretKey(accessKey, secretKey, secretName);

                if (result.IsJson)
                {
                    var appSettings = new InMemoryAppSettings();
                    var connectionStrings = new InMemoryConnectionStrings();

                    if (result.AppSettings != null)
                    {
                        foreach (var kv in result.AppSettings)
                        {
                            appSettings.Set(kv.Key, kv.Value);
                            Console.WriteLine($"[{DateTime.Now}] Set AppSetting: {kv.Key} = {kv.Value}");
                        }
                    }

                    if (result.ConnectionStringPairs != null)
                    {
                        foreach (var kv in result.ConnectionStringPairs)
                        {
                            connectionStrings.Set(kv.Key, kv.Value.ConnectionString, kv.Value.ProviderName);
                            Console.WriteLine($"[{DateTime.Now}] Set ConnectionString: {kv.Key} = {kv.Value.ConnectionString}, ProviderName: {kv.Value.ProviderName}");
                        }
                    }

                    ConfigurationManager.Instance = new ConfigManagerImpl(appSettings, connectionStrings);
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now}] Plain Text Secret: {result.PlainText}");
                }
                Console.WriteLine("-----------------------------------------------------------------");

                if (!_firstRefreshDone.Task.IsCompleted)
                    _firstRefreshDone.SetResult(true);

                await Task.Delay(TimeSpan.FromMinutes(intervalMinutes), stoppingToken);
            }
        }
    }
}
