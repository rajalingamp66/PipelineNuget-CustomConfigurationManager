using System;

namespace CustomConfigurationManager
{
    public class ConfigManagerImpl : IConfigurationManager
    {
        public IAppSettings AppSettings { get; }

        public IConnectionStrings ConnectionStrings { get; }

        public ConfigManagerImpl(IAppSettings appSettings, IConnectionStrings connectionStrings)
        {
            AppSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings), "AppSettings is null");
            ConnectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings), "ConnectionStrings is null");
        }
    }
}