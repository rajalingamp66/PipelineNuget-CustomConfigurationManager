using System;

namespace CustomConfigurationManager
{
    public class ReadConfigReport
    {
        private readonly IAppSettings _appSettings;
        private readonly IConnectionStrings _connectionStrings;
        public ReadConfigReport(IAppSettings appSettings, IConnectionStrings connectionStrings)
        {
            _appSettings = appSettings;
            _connectionStrings = connectionStrings;
        }
        public void WriteSettings()
        {
            Console.WriteLine("Settings :");
            foreach (var key in _appSettings.AllKeys)
            {
                Console.WriteLine($"Key : {key}, Value : {_appSettings[key]}");
            }
            foreach (var connectionSettings in _connectionStrings)
            {
                Console.WriteLine($"Name : {connectionSettings.Name}, Value : {connectionSettings.ConnectionString}");
            }
            Console.WriteLine("Read Config Report is Done");
        }
    }
}
