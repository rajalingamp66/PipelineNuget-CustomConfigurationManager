using System.Collections.Generic;

namespace CustomConfigurationManager
{
    public interface IConnectionStrings 
    {
        ConnectionStringSettings this[string key] { get; set; }
    }
}