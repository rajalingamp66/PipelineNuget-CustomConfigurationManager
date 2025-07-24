namespace CustomConfigurationManager
{
    public interface IConnectionStringSettings
    {
        string Name { get; }
        string ConnectionString { get; }
        string ProviderName { get; }
    }
}