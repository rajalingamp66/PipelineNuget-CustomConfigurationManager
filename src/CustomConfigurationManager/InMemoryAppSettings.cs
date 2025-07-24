using System.Collections.Concurrent;

namespace CustomConfigurationManager
{
    public class InMemoryAppSettings : IAppSettings
    {
        private readonly ConcurrentDictionary<string, string> _settings = new();

        public string Get(string key) => _settings.TryGetValue(key, out var value) ? value : null;

        public void Set(string key, string value) => _settings[key] = value;

        public string this[string key]
        {
            get => Get(key);
            set => Set(key, value);
        }
    }

}
