using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CustomConfigurationManager
{
    //internal class XmlConfigBuilder : IConfigBuilder
    //{
    //    private readonly ConfigurationFileMap _machineMap;
    //    private readonly ExeConfigurationFileMap _appMap;
    //    private readonly ExeConfigurationFileMap _webMap;

    //    public XmlConfigBuilder()
    //    {
    //        // 1. machine.config
    //        var machineConfigFilePath = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "Config", "machine.config");
    //        if (File.Exists(machineConfigFilePath))
    //            _machineMap = new ConfigurationFileMap(machineConfigFilePath);

    //        // 2. app.config
    //        var appConfigFilePath = GetAppConfigFilePath();
    //        if (!string.IsNullOrEmpty(appConfigFilePath))
    //            _appMap = new ExeConfigurationFileMap { ExeConfigFilename = appConfigFilePath };

    //        // 3. web.config
    //        var webConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "web.config");
    //        if (File.Exists(webConfigFilePath))
    //            _webMap = new ExeConfigurationFileMap { ExeConfigFilename = webConfigFilePath };
    //    }


    //    public IAppSettings GetAppSettings()
    //    {
    //        var inMemorySettings = new Dictionary<string, string>();

    //        // 1. machine.config
    //        if (_machineMap != null)
    //        {
    //            var machineConfig = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(_machineMap);
    //            CollectAppSettings(machineConfig, inMemorySettings);
    //        }

    //        // 2. app.config 
    //        if (_appMap != null)
    //        {
    //            var appConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(_appMap, ConfigurationUserLevel.None);
    //            CollectAppSettings(appConfig, inMemorySettings);
    //        }

    //        // 3. web.config
    //        if (_webMap != null)
    //        {
    //            var webConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(_webMap, ConfigurationUserLevel.None);
    //            CollectConnectionStrings(webConfig, inMemorySettings);
    //        }

    //        // 4. wrap it and return it
    //        var nameValueCollection = new NameValueCollection();
    //        foreach (var setting in inMemorySettings)
    //            nameValueCollection.Add(setting.Key, setting.Value);

    //        return new XmlAppSettings(nameValueCollection);
    //    }

    //    public IConnectionStrings GetConnectionStrings()
    //    {
    //        var inMemorySettings = new Dictionary<string, string>();

    //        // 1. machine.config
    //        if (_machineMap != null)
    //        {
    //            var machineConfig = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(_machineMap);
    //            CollectConnectionStrings(machineConfig, inMemorySettings);
    //        }

    //        // 2. app.config 
    //        if (_appMap != null)
    //        {
    //            var appConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(_appMap, ConfigurationUserLevel.None);
    //            CollectConnectionStrings(appConfig, inMemorySettings);
    //        }

    //        // 3. web.config
    //        if (_webMap != null)
    //        {
    //            var webConfig = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(_webMap, ConfigurationUserLevel.None);
    //            CollectConnectionStrings(webConfig, inMemorySettings);
    //        }

    //        // 4. wrap it and return it
    //        var connectionStrings = new ConnectionStringSettingsCollection();
    //        foreach (var setting in inMemorySettings)
    //            connectionStrings.Add(new ConnectionStringSettings(setting.Key, setting.Value));

    //        return new ConnectionStringsWrapper(connectionStrings);
    //    }


    //    #region Private Methods

    //    private static string GetAppConfigFilePath()
    //    {
    //        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.config");
    //        if (File.Exists(path))
    //            return path;

    //        var assembly = Assembly.GetEntryAssembly();

    //        // running as a class library
    //        if (assembly == null)
    //            return string.Empty;

    //        // .net framework changes the name of app.config to <assembly>.exe.config
    //        path = assembly.Location + ".config";
    //        return File.Exists(path) ? path : string.Empty;
    //    }

    //    private static void CollectAppSettings(Configuration config, Dictionary<string, string> inMemorySettings)
    //    {
    //        if (config.GetSection("appSettings") is not AppSettingsSection appSettings)
    //            return;

    //        DecryptSection(appSettings);

    //        foreach (KeyValueConfigurationElement setting in appSettings.Settings)
    //            inMemorySettings[setting.Key] = setting.Value;
    //    }

    //    private static void CollectConnectionStrings(Configuration config, Dictionary<string, string> inMemorySettings)
    //    {
    //        if (config.GetSection("connectionStrings") is not ConnectionStringsSection connectionStrings)
    //            return;

    //        DecryptSection(connectionStrings);

    //        foreach (ConnectionStringSettings setting in connectionStrings.ConnectionStrings)
    //            inMemorySettings[setting.Name] = setting.ConnectionString;
    //    }

    //    private static void DecryptSection(ConfigurationSection section)
    //    {
    //        if (section.SectionInformation.IsProtected)
    //            section.SectionInformation.UnprotectSection();
    //    }

    //    #endregion
    //}
}