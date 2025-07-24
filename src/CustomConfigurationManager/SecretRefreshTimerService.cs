using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomConfigurationManager
{
    public class SecretRefreshTimerService : IDisposable
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _secretName;
        private readonly Timer _timer;
        private bool _isDisposed;
        private bool _firstRunCompleted;

        public TaskCompletionSource<bool> FirstRefreshCompleted { get; } = new();

        public SecretRefreshTimerService(string accessKey, string secretKey, string secretName)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            _secretName = secretName;

            int intervalInMinutes = 10;
            string configValue = ConfigurationManager.AppSettings["SecretRefreshIntervalMinutes"];
            if (!string.IsNullOrEmpty(configValue) && int.TryParse(configValue, out int parsedMinutes))
                intervalInMinutes = parsedMinutes;

            _timer = new Timer(OnTimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(intervalInMinutes));
        }

        private async void OnTimerCallback(object state)
        {
            try
            {
                Console.WriteLine($"[{DateTime.Now}] Refreshing secrets...");
                var secretsService = new AWSSecretsManagerKeyReadService();
                var result = await secretsService.RetrieveSecretsManagerSecretKey(_accessKey, _secretKey, _secretName);

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

                Console.WriteLine("--------------------------------------------------");

                if (!_firstRunCompleted)
                {
                    _firstRunCompleted = true;
                    FirstRefreshCompleted.TrySetResult(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Secret refresh failed: {ex.Message}");
                if (!_firstRunCompleted)
                    FirstRefreshCompleted.TrySetException(ex);
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _timer?.Dispose();
                _isDisposed = true;
            }
        }
    }
}
