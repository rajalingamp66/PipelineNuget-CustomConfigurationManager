using System.Collections.Generic;
using System.Security.AccessControl;

namespace CustomConfigurationManager
{
    public class SecretResult
    {
        public bool IsJson { get; set; }
        //public Dictionary<string,string>? KeyValuePairs { get; set; }
        public string? PlainText { get; set; }
        public Dictionary<string, string>? AppSettings { get; set; }
        public Dictionary<string, (string ConnectionString, string ProviderName)>? ConnectionStringPairs { get; set; }
    }
}
