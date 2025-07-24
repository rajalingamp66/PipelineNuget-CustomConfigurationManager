namespace CustomConfigurationManager
{
    public interface IAppSettings
    {
        string Get(string key);
        void Set(string key, string value);
        string this[string key] { get; set; } // Add indexer
    }
}