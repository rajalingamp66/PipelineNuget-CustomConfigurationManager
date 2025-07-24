using System;

namespace CustomConfigurationManager
{
    public static class ConfigurationManager
    {
        #region Singleton
        private static readonly Lazy<IConfigurationManager> _lazy = new(() =>
        {
            var appSettings = new InMemoryAppSettings();
            var connectionStrings = new InMemoryConnectionStrings();
            return new ConfigManagerImpl(appSettings, connectionStrings);
        });

        private static IConfigurationManager _instance;

        public static IConfigurationManager Instance
        {
            set => _instance = value;
            get
            {
                if (_instance == null)
                    _instance = _lazy.Value;

                return _instance;
            }
        }
        #endregion

        public static IAppSettings AppSettings => Instance.AppSettings;

        public static IConnectionStrings ConnectionStrings => Instance.ConnectionStrings;
    }
}