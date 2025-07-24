using ClassLibrary1;
using CustomConfigurationManager;
using System;

namespace VC_Net48
{
    internal static class Program
    {
        static void Main(string[] args)
        {


            string accessKey = System.Configuration.ConfigurationManager.AppSettings["YOUR_ACCESS_KEY_ID"];
            string secretKey = System.Configuration.ConfigurationManager.AppSettings["YOUR_SECRET_ACCESS_KEY"];
            string secretName = System.Configuration.ConfigurationManager.AppSettings["SecretName"];

            using (var secretRefresher = new SecretRefreshTimerService(accessKey, secretKey, secretName))
            {
                Console.WriteLine("Waiting for initial secret load...");
                // Wait for the first refresh to complete
                secretRefresher.FirstRefreshCompleted.Task.Wait();

                var obj = new Class1();
                var message = obj.GetValues();

                Console.WriteLine("Output from .NET 4.8 console app:");
                Console.WriteLine(message);
                Console.WriteLine("\nPress any key to exit and stop refresh timer...");
                Console.ReadKey();
            }
        }
    }
}