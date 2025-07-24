namespace CustomConfigurationManager
{
    public interface IConfigurationManager
    {
        IAppSettings AppSettings { get; }
        IConnectionStrings ConnectionStrings { get; }
    }
}