using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CustomConfigurationManager
{
    //internal class ConnectionStringsWrapper : IConnectionStrings
    //{
    //    private readonly ConnectionStringSettingsCollection _connectionStrings;

    //    public ConnectionStringsWrapper(ConnectionStringSettingsCollection connectionStrings)
    //    {
    //        _connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings), "ConnectionStrings collection is null");
    //    }

    //    public IConnectionStringSettings this[string name]
    //    {
    //        get
    //        {
    //            var connectionStringSetting = _connectionStrings[name];
    //            return connectionStringSetting != null 
    //                ? new ConnectionStringSettingsWrapper(connectionStringSetting) 
    //                : null;
    //        }
    //    }

    //    public IEnumerator<IConnectionStringSettings> GetEnumerator()
    //    {
    //        var wrappers = from ConnectionStringSettings setting in _connectionStrings
    //            select new ConnectionStringSettingsWrapper(setting);

    //        return wrappers.Cast<IConnectionStringSettings>().GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //}
}