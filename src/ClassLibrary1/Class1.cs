using CustomConfigurationManager;

namespace ClassLibrary1
{
    public class Class1
    {
        public string GetValues()
        {

            var lergFilePath = ConfigurationManager.AppSettings["WfoWebServiceUrl"];
            var inCodeConnString = ConfigurationManager.ConnectionStrings?["LocalSqlServer"]?.ConnectionString;

            var key1AppSettings = ConfigurationManager.AppSettings?["WfoWebsiteUrl"];
            var key2ConnString = ConfigurationManager.ConnectionStrings?["DataWarehouseDW"]?.ConnectionString;

            var values = new List<string>
            {
                $"AWS Secrets :: AppSettings['WfoWebServiceUrl'] = {lergFilePath}",
                $"AWS Secrets :: ConnectionString['LocalSqlServer'] = {inCodeConnString}",
                "---",
                $"AWS Secrets :: AppSettings['WfoWebsiteUrl'] = {key1AppSettings}",
                $"AWS Secrets :: ConnectionString['DataWarehouseDW'] = {key2ConnString}"
            };

            return string.Join("\n", values);
        }
    }
}