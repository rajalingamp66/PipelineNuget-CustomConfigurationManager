using ClassLibrary1;
using CustomConfigurationManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<AWSSecretsManagerKeyReadService>();
        services.AddSingleton<SecretRefreshBackgroundService>(); 
        services.AddHostedService(provider => provider.GetRequiredService<SecretRefreshBackgroundService>());
    });

using var host = builder.Build();
await host.StartAsync();

// Wait until the first secrets refresh is completed
var secretRefresher = host.Services.GetRequiredService<SecretRefreshBackgroundService>();
await secretRefresher.FirstRefreshCompleted;

Console.WriteLine("Secrets loaded. Executing business logic...");

var obj = new Class1();
var message = obj.GetValues();
Console.WriteLine("Output from .NET 8.0 console app:");
Console.WriteLine(message);

// Now keep the app running
await host.WaitForShutdownAsync();
