using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace CustomConfigurationManager
{
    //internal class JsonAppSettings : IAppSettings
    //{
    //    private readonly IConfiguration _configuration;

    //    public JsonAppSettings(IConfiguration configuration)
    //    {
    //        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "AppSettings configuration is null");
    //    }

    //    public string this[string name]
    //    {
    //        get => _configuration[$"{name}"] ?? string.Empty;
    //        set => _configuration[$"{name}"] = value;
    //    }

    //    public string[] AllKeys => _configuration.GetSection("AppSettings").GetChildren().Select(c => c.Key).ToArray();

    //    public string Get(string name) => _configuration[$"AppSettings:{name}"] ?? string.Empty;
    //}
}