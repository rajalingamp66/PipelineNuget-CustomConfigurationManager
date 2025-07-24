using System.Collections.Concurrent;

namespace CustomConfigurationManager
{
    public class InMemoryConnectionStrings : IConnectionStrings
    {
        private readonly ConcurrentDictionary<string, ConnectionStringSettings> _connectionStrings = new();

        public ConnectionStringSettings this[string key]
        {
            get => _connectionStrings.TryGetValue(key, out var value) ? value : null;

            set
            {
                if (value != null)
                    _connectionStrings[key] = value;
            }
        }

        public string Get(string key) =>
            _connectionStrings.TryGetValue(key, out var value) ? value.ConnectionString : null;

        public void Set(string key, string connectionString, string providerName = null)
        {
            _connectionStrings[key] = new ConnectionStringSettings
            {
                Name = key,
                ConnectionString = connectionString,
                ProviderName = providerName
            };
        }

        // Optional: Overload for backward compatibility
        public void Set(string key, string connectionString) =>
            Set(key, connectionString, null);
    }
}
