using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace CustomConfigurationManager
{
    //internal class JsonConfigBuilder : IConfigBuilder
    //{
    //    private static readonly IDataProtectionProvider _dataProtectionProvider = DataProtectionProvider.Create("AppSettings");
    //    private readonly string _jsonFilePath;
    //    private JObject _rawJson;

    //    public JsonConfigBuilder(string jsonFilePath = null)
    //    {
    //        if (string.IsNullOrEmpty(jsonFilePath))
    //            jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

    //        _jsonFilePath = jsonFilePath;
    //    }


    //    public IAppSettings GetAppSettings()
    //    {
    //        var configurationBuilder = new ConfigurationBuilder();
    //        if (TryGetJson(out var json))
    //        {
    //            DecryptSectionValues(json, "AppSettings");
    //            var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json.ToString()));
    //            configurationBuilder.AddJsonStream(jsonStream);
    //        }

    //        var config = configurationBuilder.Build();
    //        var appSettings = config.GetSection("AppSettings");
    //        return new JsonAppSettings(appSettings);
    //    }

    //    public IConnectionStrings GetConnectionStrings()
    //    {
    //        var configurationBuilder = new ConfigurationBuilder();
    //        if (TryGetJson(out var json))
    //        {
    //            DecryptSectionValues(json, "ConnectionStrings");
    //            var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json.ToString()));
    //            configurationBuilder.AddJsonStream(jsonStream);
    //        }

    //        var config = configurationBuilder.Build();
    //        var section = config.GetSection("ConnectionStrings");
    //        var connectionStrings = new ConnectionStringSettingsCollection();
    //        foreach (var setting in section.GetChildren())
    //            connectionStrings.Add(new ConnectionStringSettings(setting.Key, setting.Value));

    //        return new ConnectionStringsWrapper(connectionStrings);
    //    }


    //    #region Private Methods

    //    private bool TryGetJson(out JObject result)
    //    {
    //        result = null;

    //        // optimization
    //        if (!File.Exists(_jsonFilePath))
    //            return false;

    //        // load & cache
    //        if (_rawJson == null)
    //        {
    //            var jsonContent = File.ReadAllText(_jsonFilePath);
    //            _rawJson = JObject.Parse(jsonContent);
    //        }

    //        result = _rawJson;
    //        return _rawJson != null;
    //    }

    //    private static void DecryptSectionValues(JObject jsonObj, string sectionKey)
    //    {
    //        if (jsonObj[sectionKey] is not JObject sectionObject)
    //            return;

    //        var stringProperties = (from s in sectionObject.Properties()
    //                                where s.Value.Type == JTokenType.String
    //                                select s).ToArray();

    //        foreach (var property in stringProperties)
    //        {
    //            if (TryDecrypt(property.Value.ToString(), out var result))
    //                property.Value = result;
    //        }
    //    }

    //    private static bool TryDecrypt(string value, out string result)
    //    {
    //        result = null;

    //        if (string.IsNullOrEmpty(value))
    //            return false;

    //        try
    //        {
    //            var encryptedBytes = Convert.FromBase64String(value);
    //            var dataProtector = _dataProtectionProvider.CreateProtector("AppSettingsProtector");
    //            var decryptedBytes = dataProtector.Unprotect(encryptedBytes);

    //            result = Encoding.UTF8.GetString(decryptedBytes);
    //            return !string.IsNullOrEmpty(result);
    //        }
    //        catch
    //        {
    //            // noop
    //            return false;
    //        }
    //    }

    //    #endregion
    //}
}